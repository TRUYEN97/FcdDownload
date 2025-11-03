namespace CPEI_MFG.View
{
    partial class UnitView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grMain = new System.Windows.Forms.GroupBox();
            this.btVerify = new System.Windows.Forms.Button();
            this.lbInput = new System.Windows.Forms.Label();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.lbFail = new System.Windows.Forms.Label();
            this.lbProbe = new System.Windows.Forms.Label();
            this.lbPass = new System.Windows.Forms.Label();
            this.lbRJ45 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.lbStatus = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.grMain.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.SuspendLayout();
            // 
            // grMain
            // 
            this.grMain.Controls.Add(this.btVerify);
            this.grMain.Controls.Add(this.lbInput);
            this.grMain.Controls.Add(this.groupBox13);
            this.grMain.Controls.Add(this.txtInput);
            this.grMain.Controls.Add(this.lbStatus);
            this.grMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grMain.Location = new System.Drawing.Point(0, 0);
            this.grMain.Name = "grMain";
            this.grMain.Size = new System.Drawing.Size(824, 123);
            this.grMain.TabIndex = 19;
            this.grMain.TabStop = false;
            this.grMain.Text = "SLOT 0";
            // 
            // btVerify
            // 
            this.btVerify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btVerify.Location = new System.Drawing.Point(546, 47);
            this.btVerify.Name = "btVerify";
            this.btVerify.Size = new System.Drawing.Size(62, 31);
            this.btVerify.TabIndex = 18;
            this.btVerify.TabStop = false;
            this.btVerify.Text = "Verify";
            this.btVerify.UseVisualStyleBackColor = true;
            this.btVerify.Click += new System.EventHandler(this.btVerify_Click);
            // 
            // lbInput
            // 
            this.lbInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbInput.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbInput.Location = new System.Drawing.Point(9, 19);
            this.lbInput.Name = "lbInput";
            this.lbInput.Size = new System.Drawing.Size(599, 25);
            this.lbInput.TabIndex = 17;
            // 
            // groupBox13
            // 
            this.groupBox13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox13.Controls.Add(this.lbFail);
            this.groupBox13.Controls.Add(this.lbProbe);
            this.groupBox13.Controls.Add(this.lbPass);
            this.groupBox13.Controls.Add(this.lbRJ45);
            this.groupBox13.Controls.Add(this.label13);
            this.groupBox13.Controls.Add(this.label11);
            this.groupBox13.Controls.Add(this.label9);
            this.groupBox13.Controls.Add(this.label7);
            this.groupBox13.Location = new System.Drawing.Point(614, 19);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(204, 96);
            this.groupBox13.TabIndex = 15;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Data Monitor";
            // 
            // lbFail
            // 
            this.lbFail.AutoSize = true;
            this.lbFail.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFail.Location = new System.Drawing.Point(146, 69);
            this.lbFail.Name = "lbFail";
            this.lbFail.Size = new System.Drawing.Size(17, 19);
            this.lbFail.TabIndex = 8;
            this.lbFail.Text = "0";
            // 
            // lbProbe
            // 
            this.lbProbe.AutoSize = true;
            this.lbProbe.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbProbe.Location = new System.Drawing.Point(146, 28);
            this.lbProbe.Name = "lbProbe";
            this.lbProbe.Size = new System.Drawing.Size(17, 19);
            this.lbProbe.TabIndex = 7;
            this.lbProbe.Text = "0";
            this.lbProbe.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbProbe_MouseDoubleClick);
            // 
            // lbPass
            // 
            this.lbPass.AutoSize = true;
            this.lbPass.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPass.Location = new System.Drawing.Point(55, 69);
            this.lbPass.Name = "lbPass";
            this.lbPass.Size = new System.Drawing.Size(17, 19);
            this.lbPass.TabIndex = 6;
            this.lbPass.Text = "0";
            // 
            // lbRJ45
            // 
            this.lbRJ45.AutoSize = true;
            this.lbRJ45.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRJ45.Location = new System.Drawing.Point(55, 28);
            this.lbRJ45.Name = "lbRJ45";
            this.lbRJ45.Size = new System.Drawing.Size(17, 19);
            this.lbRJ45.TabIndex = 5;
            this.lbRJ45.Text = "0";
            this.lbRJ45.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbRJ45_MouseDoubleClick);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(104, 69);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(39, 19);
            this.label13.TabIndex = 4;
            this.label13.Text = "FAIL:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(104, 28);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 19);
            this.label11.TabIndex = 3;
            this.label11.Text = "Probe:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(16, 69);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 19);
            this.label9.TabIndex = 2;
            this.label9.Text = "PASS:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(17, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 19);
            this.label7.TabIndex = 1;
            this.label7.Text = "RJ45:";
            // 
            // txtInput
            // 
            this.txtInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInput.Location = new System.Drawing.Point(9, 47);
            this.txtInput.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(532, 31);
            this.txtInput.TabIndex = 0;
            this.txtInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInput_KeyPress);
            // 
            // lbStatus
            // 
            this.lbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbStatus.Location = new System.Drawing.Point(9, 81);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(599, 34);
            this.lbStatus.TabIndex = 14;
            this.lbStatus.Text = "Waiting";
            this.lbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UnitView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grMain);
            this.Name = "UnitView";
            this.Size = new System.Drawing.Size(824, 123);
            this.grMain.ResumeLayout(false);
            this.grMain.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grMain;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.Label lbFail;
        private System.Windows.Forms.Label lbProbe;
        private System.Windows.Forms.Label lbPass;
        private System.Windows.Forms.Label lbRJ45;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label lbInput;
        private System.Windows.Forms.Button btVerify;
    }
}
