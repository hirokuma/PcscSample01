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
using PCSC.Iso7816;

namespace PcscSample01
{
    public partial class PcscSample01 : Form
    {
        SCardContext mContext;
        SCardReader mReader;

        //アプリケーション識別子
        byte[][] AID = new byte[][]
        {
            new byte[] { 0xa0, 0x00, 0x00, 0x02, 0x31, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
            new byte[] { 0xa0, 0x00, 0x00, 0x02, 0x31, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
            new byte[] { 0xa0, 0x00, 0x00, 0x02, 0x48, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }
        };
        //byte[] AID_DF1 = new byte[] { 0xa0, 0x00, 0x00, 0x02, 0x31, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        //byte[] AID_DF2 = new byte[] { 0xa0, 0x00, 0x00, 0x02, 0x31, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        //byte[] AID_DF3 = new byte[] { 0xa0, 0x00, 0x00, 0x02, 0x48, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        public PcscSample01()
        {
            InitializeComponent();

            //Enabled=false
            foreach (Control ctrl in groupControl.Controls)
            {
                ctrl.Enabled = false;
            }

            //リソースマネージャとの接続
            mContext = new SCardContext();
            mContext.Establish(SCardScope.User);
            mReader = new SCardReader(mContext);

            int num = updateComboRw();
            buttonConnect.Enabled = (num != 0);

            //コンボボックス
            comboDF.SelectedIndex = 0;
            updateComboEF();
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

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            byte[] aid = null;
            int index;

            //Dedicated
            if (comboDF.SelectedIndex == 0)
            {
                //MF
                aid = null;
                index = 0x2f01;
            }
            else
            {
                aid = AID[comboDF.SelectedIndex - 1];
                index = comboEF.SelectedIndex;
            }
            sendSelectFile(mReader, FileType.Dedicated, aid);


            if (comboEF.SelectedIndex != 0)
            {
                //Elementary

                sendSelectFile(mReader, index);
            }
        }

        private void buttonVerifyNum1_Click(object sender, EventArgs e)
        {
            sendVerifyNum(mReader, 1);
        }

        private void buttonVerifyNum2_Click(object sender, EventArgs e)
        {
            sendVerifyNum(mReader, 2);
        }

        private void comboDF_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateComboEF();
        }

        private void buttonReadBinary_Click(object sender, EventArgs e)
        {
            if (comboEF.SelectedIndex == 0)
            {
                MessageBox.Show("not Select EF");
                return;
            }

            sendReadBinary(mReader);
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

        private void updateComboEF()
        {
            comboEF.Items.Clear();
            comboEF.Items.Add("Dedicate");

            switch (comboDF.SelectedIndex)
            {
                case 0: //MF
                    comboEF.Items.Add("EF01");
                    break;
                case 1: //DF1
                    comboEF.Items.Add("EF01");
                    comboEF.Items.Add("EF02");
                    comboEF.Items.Add("EF03");
                    comboEF.Items.Add("EF04");
                    comboEF.Items.Add("EF05");
                    comboEF.Items.Add("EF06");
                    comboEF.Items.Add("EF07");
                    break;
                case 2: //DF2
                    comboEF.Items.Add("EF01");
                    break;
                case 3: //DF3
                    comboEF.Items.Add("EF01");
                    break;
            }
            comboEF.SelectedIndex = 0;
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
                MessageBox.Show("Reader : " + readerNames[0] +
                    "\nProtocol : " + proto +
                    "\nState : " + state +
                    "\nATR : " + BitConverter.ToString(atr),
                    "STATUS");
            }
            else
            {
                MessageBox.Show("Fail Status.", "STATUS");
            }
        }

        private void sendGetData(ISCardReader reader)
        {
            SCardError err;

            err = reader.BeginTransaction();
            if (err != SCardError.Success)
            {
                MessageBox.Show("Fail BeginTransaction.", "BEGIN TRANSACTION");
                return;
            }

            // pcsc3_v2.01.09.pdf
            // p.31
            // "3.2.2.1.3  Get Data Command"
#if true
            CommandApdu apdu = new CommandApdu(IsoCase.Case2Short, reader.ActiveProtocol)
            {
                CLA = 0xff,
                Instruction = InstructionCode.GetData,
                P1 = 0x00,
                P2 = 0x00,
                Le = 0
            };
            SCardPCI recvPci = new SCardPCI();      //protocol control information
            byte[] res = new byte[10];
            err = reader.Transmit(
                SCardPCI.GetPci(reader.ActiveProtocol),
                apdu.ToArray(),
                recvPci,
                ref res);
#else
            byte[] cmd = { 0xff, 0xca, 0x00, 0x00, 0x00 };
            byte[] res = new byte[10];
            err = reader.Transmit(cmd, ref res);
#endif
            reader.EndTransaction(SCardReaderDisposition.Leave);

            if (err == SCardError.Success)
            {
#if true
                ResponseApdu resApdu = new ResponseApdu(res, IsoCase.Case2Short, reader.ActiveProtocol);
                MessageBox.Show("result : " + BitConverter.ToString(resApdu.GetData()) +
                    "\nSW1 : " + resApdu.SW1.ToString("x2") +
                    "\nSW2 : " + resApdu.SW2.ToString("x2"),
                    "GET DATA");
#else
                MessageBox.Show("result : " + BitConverter.ToString(res, 0, res.Length - 2) +
                    "\nSW1 : " + res[res.Length - 2].ToString("x2") + 
                    "\nSW2 : " + res[res.Length - 1].ToString("x2"));
#endif
            }
            else
            {
                MessageBox.Show("Fail Transmit", "TRANSMIT");
            }
        }

        private void sendSelectFile(ISCardReader reader, int ef)
        {
            byte[] efid = new byte[2];

            efid[0] = (byte)((ef & 0xff00) >> 8);
            efid[1] = (byte)( ef & 0x00ff);
            sendSelectFile(reader, FileType.Elementary, efid);
        }

        private void sendSelectFile(ISCardReader reader, FileType type, byte[] aid)
        {
            SCardError err;
            string typeName = "";

            err = reader.BeginTransaction();
            if (err != SCardError.Success)
            {
                MessageBox.Show("Fail BeginTransaction.", "BEGIN TRANSACTION");
                return;
            }

            CommandApdu apdu = null;
            switch (type)
            {
                case FileType.Dedicated:
                    if (aid == null)
                    {
                        //MF
                        apdu = new CommandApdu(IsoCase.Case1, reader.ActiveProtocol)
                        {
                            P1 = 0x00,
                            P2 = 0x00       //MFはこれでもFCI無し
                        };
                        typeName = "Dedicated(MF)";
                    }
                    else
                    {
                        //DF
                        apdu = new CommandApdu(IsoCase.Case3Short, reader.ActiveProtocol)
                        {
                            P1 = 0x04,
                            P2 = 0x0c,      //FCI無し
                            Data = aid,
                        };
                        typeName = "Dedicated(DF)";
                    }
                    break;

                case FileType.Elementary:
                    //EF
                    apdu = new CommandApdu(IsoCase.Case3Short, reader.ActiveProtocol)
                    {
                        P1 = 0x02,
                        P2 = 0x0c,          //FCI無し
                        Data = aid
                    };
                    typeName = "Elemantary";
                    break;
            }
            apdu.Instruction = InstructionCode.SelectFile;
            SCardPCI recvPci = new SCardPCI();      //protocol control information
            byte[] res = new byte[2];
            err = reader.Transmit(
                SCardPCI.GetPci(reader.ActiveProtocol),
                apdu.ToArray(),
                recvPci,
                ref res);
            reader.EndTransaction(SCardReaderDisposition.Leave);

            if (err == SCardError.Success)
            {
                ResponseApdu resApdu = new ResponseApdu(res, IsoCase.Case2Short, reader.ActiveProtocol);
                MessageBox.Show(
                    "\nSW1 : " + resApdu.SW1.ToString("x2") +
                    "\nSW2 : " + resApdu.SW2.ToString("x2"),
                    "SELECT FILE : " + typeName);
            }
            else
            {
                MessageBox.Show("Fail Transmit");
            }
        }

        private void sendVerifyNum(ISCardReader reader, int ef)
        {
            SCardError err;

            if ((ef != 1) && (ef != 2))
            {
                MessageBox.Show("Invalid Args.", "err");
                return;
            }

            err = reader.BeginTransaction();
            if (err != SCardError.Success)
            {
                MessageBox.Show("Fail BeginTransaction.", "BEGIN TRANSACTION");
                return;
            }

            CommandApdu apdu = new CommandApdu(IsoCase.Case1, reader.ActiveProtocol)
            {
                CLA = 0x00,
                Instruction = InstructionCode.Verify,
                P1 = 0x00,
                P2 = (byte)(0x80 + ef)          //短縮EF識別子指定
            };

            SCardPCI recvPci = new SCardPCI();      //protocol control information
            byte[] res = new byte[2];
            err = reader.Transmit(
                SCardPCI.GetPci(reader.ActiveProtocol),
                apdu.ToArray(),
                recvPci,
                ref res);
            reader.EndTransaction(SCardReaderDisposition.Leave);

            if (err == SCardError.Success)
            {
                ResponseApdu resApdu = new ResponseApdu(res, IsoCase.Case2Short, reader.ActiveProtocol);
                MessageBox.Show(
                    "SW1 : " + resApdu.SW1.ToString("x2") +
                    "\nSW2 : " + resApdu.SW2.ToString("x2"),
                    "Verify(Num)");
            }
            else
            {
                MessageBox.Show("Fail Transmit");
            }
        }


        private void sendReadBinary(ISCardReader reader)
        {
            SCardError err;
            IsoCase iso_case = IsoCase.Case2Short;

            err = reader.BeginTransaction();
            if (err != SCardError.Success)
            {
                MessageBox.Show("Fail BeginTransaction.", "BEGIN TRANSACTION");
                return;
            }

            CommandApdu apdu = new CommandApdu(iso_case, reader.ActiveProtocol)
            {
                CLA = 0x00,
                Instruction = InstructionCode.ReadBinary,
                //P1 = 0x80,      //相対アドレス8bit指定 + カレントEF指定
                //P2 = 0x00
                P1 = 0x00,      //相対アドレス15bit指定
                P2 = 0x00
            };

            SCardPCI recvPci = new SCardPCI();      //protocol control information
            byte[] res = new byte[880];
            err = reader.Transmit(
                SCardPCI.GetPci(reader.ActiveProtocol),
                apdu.ToArray(),
                recvPci,
                ref res);
            reader.EndTransaction(SCardReaderDisposition.Leave);

            if (err == SCardError.Success)
            {
                ResponseApdu resApdu = new ResponseApdu(res, iso_case, reader.ActiveProtocol);
                string data = "(none)";
                if (resApdu.DataSize > 0)
                {
                    data = BitConverter.ToString(resApdu.GetData());
                }
                MessageBox.Show("result : " + data +
                    "\nSW1 : " + resApdu.SW1.ToString("x2") +
                    "\nSW2 : " + resApdu.SW2.ToString("x2"),
                    "ReadBinary");
            }
            else
            {
                MessageBox.Show("Fail Transmit");
            }
        }
    }
}
