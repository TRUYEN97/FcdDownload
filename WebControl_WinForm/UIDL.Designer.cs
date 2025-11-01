namespace WebControl_WinForm
{
    partial class UIDL
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label_Model = new System.Windows.Forms.Label();
            this.label_Station = new System.Windows.Forms.Label();
            this.label_StationNO = new System.Windows.Forms.Label();
            this.grMain = new System.Windows.Forms.GroupBox();
            this.pnMain = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grMain.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(988, 31);
            this.label1.TabIndex = 5;
            this.label1.Text = "UI Download Program";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Model
            // 
            this.label_Model.AutoSize = true;
            this.label_Model.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Model.ForeColor = System.Drawing.Color.Blue;
            this.label_Model.Location = new System.Drawing.Point(7, 32);
            this.label_Model.Name = "label_Model";
            this.label_Model.Size = new System.Drawing.Size(174, 31);
            this.label_Model.TabIndex = 18;
            this.label_Model.Text = "Model_Name";
            // 
            // label_Station
            // 
            this.label_Station.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Station.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Station.ForeColor = System.Drawing.Color.Blue;
            this.label_Station.Location = new System.Drawing.Point(6, 33);
            this.label_Station.Name = "label_Station";
            this.label_Station.Size = new System.Drawing.Size(701, 29);
            this.label_Station.TabIndex = 19;
            this.label_Station.Text = "Station";
            this.label_Station.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_StationNO
            // 
            this.label_StationNO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label_StationNO.AutoSize = true;
            this.label_StationNO.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_StationNO.ForeColor = System.Drawing.Color.Blue;
            this.label_StationNO.Location = new System.Drawing.Point(581, 58);
            this.label_StationNO.Name = "label_StationNO";
            this.label_StationNO.Size = new System.Drawing.Size(83, 20);
            this.label_StationNO.TabIndex = 20;
            this.label_StationNO.Text = "StationNO";
            // 
            // grMain
            // 
            this.grMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grMain.Controls.Add(this.pnMain);
            this.grMain.Location = new System.Drawing.Point(12, 159);
            this.grMain.Name = "grMain";
            this.grMain.Size = new System.Drawing.Size(992, 554);
            this.grMain.TabIndex = 21;
            this.grMain.TabStop = false;
            this.grMain.Text = "Main process";
            // 
            // pnMain
            // 
            this.pnMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.pnMain.ColumnCount = 1;
            this.pnMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnMain.Location = new System.Drawing.Point(3, 16);
            this.pnMain.Name = "pnMain";
            this.pnMain.RowCount = 1;
            this.pnMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnMain.Size = new System.Drawing.Size(986, 535);
            this.pnMain.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label_Model);
            this.groupBox2.Location = new System.Drawing.Point(13, 60);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(265, 93);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Model";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.label_StationNO);
            this.groupBox3.Controls.Add(this.label_Station);
            this.groupBox3.Location = new System.Drawing.Point(284, 60);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(723, 93);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Station";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(13, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(994, 43);
            this.panel1.TabIndex = 25;
            // 
            // UIDL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSkyBlue;
            this.ClientSize = new System.Drawing.Size(1016, 725);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grMain);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "UIDL";
            this.Text = "UI Download";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UIDL_FormClosed);
            this.Load += new System.EventHandler(this.Form_Load);
            this.grMain.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_Model;
        private System.Windows.Forms.Label label_Station;
        private System.Windows.Forms.Label label_StationNO;
        private System.Windows.Forms.GroupBox grMain;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel pnMain;
    }
}

