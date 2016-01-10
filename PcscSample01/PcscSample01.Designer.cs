namespace PcscSample01
{
    partial class PcscSample01
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonRwDetect = new System.Windows.Forms.Button();
            this.comboRw = new System.Windows.Forms.ComboBox();
            this.buttonStatus = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.groupControl = new System.Windows.Forms.GroupBox();
            this.buttonGetData = new System.Windows.Forms.Button();
            this.buttonSelectMF = new System.Windows.Forms.Button();
            this.buttonVerifyNum1 = new System.Windows.Forms.Button();
            this.buttonVerifyNum2 = new System.Windows.Forms.Button();
            this.groupControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonRwDetect
            // 
            this.buttonRwDetect.Location = new System.Drawing.Point(295, 10);
            this.buttonRwDetect.Name = "buttonRwDetect";
            this.buttonRwDetect.Size = new System.Drawing.Size(75, 23);
            this.buttonRwDetect.TabIndex = 0;
            this.buttonRwDetect.Text = "R/W detect";
            this.buttonRwDetect.UseVisualStyleBackColor = true;
            this.buttonRwDetect.Click += new System.EventHandler(this.buttonRwDetect_Click);
            // 
            // comboRw
            // 
            this.comboRw.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRw.FormattingEnabled = true;
            this.comboRw.Location = new System.Drawing.Point(12, 12);
            this.comboRw.Name = "comboRw";
            this.comboRw.Size = new System.Drawing.Size(277, 20);
            this.comboRw.TabIndex = 1;
            // 
            // buttonStatus
            // 
            this.buttonStatus.Enabled = false;
            this.buttonStatus.Location = new System.Drawing.Point(6, 18);
            this.buttonStatus.Name = "buttonStatus";
            this.buttonStatus.Size = new System.Drawing.Size(75, 23);
            this.buttonStatus.TabIndex = 2;
            this.buttonStatus.Text = "Status";
            this.buttonStatus.UseVisualStyleBackColor = true;
            this.buttonStatus.Click += new System.EventHandler(this.buttonStatus_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Enabled = false;
            this.buttonConnect.Location = new System.Drawing.Point(12, 38);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(358, 23);
            this.buttonConnect.TabIndex = 3;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // groupControl
            // 
            this.groupControl.Controls.Add(this.buttonVerifyNum2);
            this.groupControl.Controls.Add(this.buttonVerifyNum1);
            this.groupControl.Controls.Add(this.buttonSelectMF);
            this.groupControl.Controls.Add(this.buttonGetData);
            this.groupControl.Controls.Add(this.buttonStatus);
            this.groupControl.Location = new System.Drawing.Point(12, 67);
            this.groupControl.Name = "groupControl";
            this.groupControl.Size = new System.Drawing.Size(358, 194);
            this.groupControl.TabIndex = 4;
            this.groupControl.TabStop = false;
            this.groupControl.Text = "Card Control";
            // 
            // buttonGetData
            // 
            this.buttonGetData.Location = new System.Drawing.Point(87, 18);
            this.buttonGetData.Name = "buttonGetData";
            this.buttonGetData.Size = new System.Drawing.Size(75, 23);
            this.buttonGetData.TabIndex = 3;
            this.buttonGetData.Text = "GetData";
            this.buttonGetData.UseVisualStyleBackColor = true;
            this.buttonGetData.Click += new System.EventHandler(this.buttonGetData_Click);
            // 
            // buttonSelectMF
            // 
            this.buttonSelectMF.Location = new System.Drawing.Point(6, 91);
            this.buttonSelectMF.Name = "buttonSelectMF";
            this.buttonSelectMF.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectMF.TabIndex = 4;
            this.buttonSelectMF.Text = "Select MF";
            this.buttonSelectMF.UseVisualStyleBackColor = true;
            this.buttonSelectMF.Click += new System.EventHandler(this.buttonSelectMF_Click);
            // 
            // buttonVerifyNum1
            // 
            this.buttonVerifyNum1.Location = new System.Drawing.Point(87, 91);
            this.buttonVerifyNum1.Name = "buttonVerifyNum1";
            this.buttonVerifyNum1.Size = new System.Drawing.Size(86, 23);
            this.buttonVerifyNum1.TabIndex = 5;
            this.buttonVerifyNum1.Text = "Verify1 num";
            this.buttonVerifyNum1.UseVisualStyleBackColor = true;
            this.buttonVerifyNum1.Click += new System.EventHandler(this.buttonVerifyNum1_Click);
            // 
            // buttonVerifyNum2
            // 
            this.buttonVerifyNum2.Location = new System.Drawing.Point(87, 120);
            this.buttonVerifyNum2.Name = "buttonVerifyNum2";
            this.buttonVerifyNum2.Size = new System.Drawing.Size(86, 23);
            this.buttonVerifyNum2.TabIndex = 6;
            this.buttonVerifyNum2.Text = "Verify2 Num";
            this.buttonVerifyNum2.UseVisualStyleBackColor = true;
            this.buttonVerifyNum2.Click += new System.EventHandler(this.buttonVerifyNum2_Click);
            // 
            // PcscSample01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 273);
            this.Controls.Add(this.groupControl);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.comboRw);
            this.Controls.Add(this.buttonRwDetect);
            this.Name = "PcscSample01";
            this.Text = "PC/SC Sample01";
            this.groupControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonRwDetect;
        private System.Windows.Forms.ComboBox comboRw;
        private System.Windows.Forms.Button buttonStatus;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.GroupBox groupControl;
        private System.Windows.Forms.Button buttonGetData;
        private System.Windows.Forms.Button buttonSelectMF;
        private System.Windows.Forms.Button buttonVerifyNum2;
        private System.Windows.Forms.Button buttonVerifyNum1;
    }
}

