using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//https://github.com/danm-de/pcsc-sharp
//https://danm.de/docs/pcsc-sharp/index.html
using PCSC;

namespace PcscSample01
{
    public partial class PcscSample01 : Form
    {
        SCardContext mContext;
        SCardReader mReader;

        public PcscSample01()
        {
            InitializeComponent();

            //リソースマネージャとの接続
            mContext = new SCardContext();
            mContext.Establish(SCardScope.User);
            mReader = new SCardReader(mContext);

            int num = updateComboRw();
            buttonConnect.Enabled = (num != 0);
        }

        private void buttonRwDetect_Click(object sender, EventArgs e)
        {
            int num = updateComboRw();
            buttonConnect.Enabled = (num != 0);
            if (num == 0)
            {
                MessageBox.Show("R/W not found", "NOT FOUND", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (!mReader.IsConnected)
            {
                //接続
                SCardError err = mReader.Connect((string)comboRw.SelectedItem, SCardShareMode.Shared, SCardProtocol.Any);
                if (err == SCardError.Success)
                {
                    buttonConnect.Text = "Disconnect";
                    buttonRwDetect.Enabled = false;
                    comboRw.Enabled = false;

                    foreach (Control ctrl in groupControl.Controls)
                    {
                        ctrl.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("Fail connect.");
                }
            }
            else
            {
                //切断
                SCardError err = mReader.Disconnect(SCardReaderDisposition.Leave);
                if (err == SCardError.Success)
                {
                    buttonConnect.Text = "Connect";
                    buttonRwDetect.Enabled = true;
                    comboRw.Enabled = true;

                    foreach (Control ctrl in groupControl.Controls)
                    {
                        ctrl.Enabled = false;
                    }
                }
                else
                {
                    MessageBox.Show("Fail disconnect.");
                }
            }
        }

        private void buttonStatus_Click(object sender, EventArgs e)
        {
            displayReaderStatus(mReader);
        }


        private void buttonGetData_Click(object sender, EventArgs e)
        {
            sendGetData(mReader);
        }

        ///////////////////////////////////////////////////////////////////////////////////

        private int updateComboRw()
        {
            comboRw.Items.Clear();
            string[] readerNames = mContext.GetReaders();
            if ((readerNames == null) || (readerNames.Length == 0))
            {
                return 0;
            }

            foreach (string reader in readerNames)
            {
                comboRw.Items.Add(reader);
            }
            comboRw.SelectedIndex = 0;

            return readerNames.Length;
        }

        private void displayReaderStatus(ISCardReader reader)
        {
            string[] readerNames;
            SCardProtocol proto;
            SCardState state;
            byte[] atr;
            SCardError err;

            err = reader.Status(out readerNames, out state, out proto, out atr);
            if (err == SCardError.Success)
            {
                MessageBox.Show("Reader : " + readerNames[0] + "\nProtocol : " + proto + "\nState : " + state + "\nATR : " + BitConverter.ToString(atr));
            }
            else
            {
                MessageBox.Show("Fail Status.");
            }
        }

        private void sendGetData(ISCardReader reader)
        {
            SCardError err;

            err = reader.BeginTransaction();
            if (err != SCardError.Success)
            {
                MessageBox.Show("Fail BeginTransaction.");
                return;
            }

            // pcsc3_v2.01.09.pdf
            // p.31
            // "3.2.2.1.3  Get Data Command"
            byte[] cmd = { 0xff, 0xca, 0x00, 0x00, 0x00 };
            byte[] res = new byte[10];
            err = reader.Transmit(cmd, ref res);

            reader.EndTransaction(SCardReaderDisposition.Leave);

            if (err == SCardError.Success)
            {
                MessageBox.Show("result : " + BitConverter.ToString(res, 0, res.Length - 2) +
                    "\nSW1 : " + res[res.Length - 2].ToString("x2") + 
                    "\nSW2 : " + res[res.Length - 1].ToString("x2"));
            }
            else
            {
                MessageBox.Show("Fail Transmit");
            }
        }
    }
}
