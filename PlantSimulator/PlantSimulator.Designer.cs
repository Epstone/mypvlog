namespace PlantSimulator
{
    partial class PlantSimulator
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
      this.tbxUrl = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.btnStart = new System.Windows.Forms.Button();
      this.btnStop = new System.Windows.Forms.Button();
      this.tbxLog = new System.Windows.Forms.TextBox();
      this.cmbxInverterCount = new System.Windows.Forms.ComboBox();
      this.cmbxLogFormat = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.tbxPlantId = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.tbxPassword = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.tb_wattage = new System.Windows.Forms.TrackBar();
      this.label6 = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.tbx_temperature = new System.Windows.Forms.TextBox();
      this.tbx_currentWattage = new System.Windows.Forms.TextBox();
      this.tb_temperature = new System.Windows.Forms.TrackBar();
      this.label7 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.tb_wattage)).BeginInit();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.tb_temperature)).BeginInit();
      this.SuspendLayout();
      // 
      // tbxUrl
      // 
      this.tbxUrl.Location = new System.Drawing.Point(12, 36);
      this.tbxUrl.Name = "tbxUrl";
      this.tbxUrl.Size = new System.Drawing.Size(250, 20);
      this.tbxUrl.TabIndex = 0;
      this.tbxUrl.Text = "http://mypvlog.de/Log/";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(9, 20);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(70, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Logging URL";
      // 
      // btnStart
      // 
      this.btnStart.Location = new System.Drawing.Point(12, 59);
      this.btnStart.Name = "btnStart";
      this.btnStart.Size = new System.Drawing.Size(75, 23);
      this.btnStart.TabIndex = 2;
      this.btnStart.Text = "Start";
      this.btnStart.UseVisualStyleBackColor = true;
      this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
      // 
      // btnStop
      // 
      this.btnStop.Location = new System.Drawing.Point(112, 59);
      this.btnStop.Name = "btnStop";
      this.btnStop.Size = new System.Drawing.Size(75, 23);
      this.btnStop.TabIndex = 3;
      this.btnStop.Text = "Stop";
      this.btnStop.UseVisualStyleBackColor = true;
      this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
      // 
      // tbxLog
      // 
      this.tbxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tbxLog.Location = new System.Drawing.Point(12, 209);
      this.tbxLog.Multiline = true;
      this.tbxLog.Name = "tbxLog";
      this.tbxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.tbxLog.Size = new System.Drawing.Size(734, 338);
      this.tbxLog.TabIndex = 4;
      // 
      // cmbxInverterCount
      // 
      this.cmbxInverterCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbxInverterCount.FormattingEnabled = true;
      this.cmbxInverterCount.Location = new System.Drawing.Point(268, 36);
      this.cmbxInverterCount.Name = "cmbxInverterCount";
      this.cmbxInverterCount.Size = new System.Drawing.Size(121, 21);
      this.cmbxInverterCount.TabIndex = 6;
      // 
      // cmbxLogFormat
      // 
      this.cmbxLogFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbxLogFormat.FormattingEnabled = true;
      this.cmbxLogFormat.Location = new System.Drawing.Point(395, 35);
      this.cmbxLogFormat.Name = "cmbxLogFormat";
      this.cmbxLogFormat.Size = new System.Drawing.Size(121, 21);
      this.cmbxLogFormat.TabIndex = 7;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(265, 16);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(113, 13);
      this.label2.TabIndex = 8;
      this.label2.Text = "Wechselrichter Anzahl";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(392, 16);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(54, 13);
      this.label3.TabIndex = 9;
      this.label3.Text = "Logformat";
      // 
      // tbxPlantId
      // 
      this.tbxPlantId.Location = new System.Drawing.Point(618, 35);
      this.tbxPlantId.Name = "tbxPlantId";
      this.tbxPlantId.Size = new System.Drawing.Size(100, 20);
      this.tbxPlantId.TabIndex = 10;
      this.tbxPlantId.Text = "1";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(520, 64);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(92, 13);
      this.label4.TabIndex = 11;
      this.label4.Text = "Anlagen Passwort";
      // 
      // tbxPassword
      // 
      this.tbxPassword.Location = new System.Drawing.Point(618, 61);
      this.tbxPassword.Name = "tbxPassword";
      this.tbxPassword.Size = new System.Drawing.Size(100, 20);
      this.tbxPassword.TabIndex = 12;
      this.tbxPassword.Text = "12345";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(552, 39);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(60, 13);
      this.label5.TabIndex = 13;
      this.label5.Text = "Anlagen ID";
      // 
      // tb_wattage
      // 
      this.tb_wattage.LargeChange = 1000;
      this.tb_wattage.Location = new System.Drawing.Point(9, 38);
      this.tb_wattage.Name = "tb_wattage";
      this.tb_wattage.Size = new System.Drawing.Size(241, 45);
      this.tb_wattage.TabIndex = 14;
      this.tb_wattage.TickFrequency = 10;
      this.tb_wattage.ValueChanged += new System.EventHandler(this.tb_wattage_ValueChanged);
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(6, 22);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(88, 13);
      this.label6.TabIndex = 15;
      this.label6.Text = "Einspeiseleistung";
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.tbx_temperature);
      this.groupBox1.Controls.Add(this.tbx_currentWattage);
      this.groupBox1.Controls.Add(this.tb_temperature);
      this.groupBox1.Controls.Add(this.label7);
      this.groupBox1.Controls.Add(this.tb_wattage);
      this.groupBox1.Controls.Add(this.label6);
      this.groupBox1.Location = new System.Drawing.Point(12, 88);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(731, 116);
      this.groupBox1.TabIndex = 16;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Messwerte";
      // 
      // tbx_temperature
      // 
      this.tbx_temperature.Enabled = false;
      this.tbx_temperature.Location = new System.Drawing.Point(256, 74);
      this.tbx_temperature.Name = "tbx_temperature";
      this.tbx_temperature.Size = new System.Drawing.Size(75, 20);
      this.tbx_temperature.TabIndex = 19;
      // 
      // tbx_currentWattage
      // 
      this.tbx_currentWattage.Enabled = false;
      this.tbx_currentWattage.Location = new System.Drawing.Point(9, 74);
      this.tbx_currentWattage.Name = "tbx_currentWattage";
      this.tbx_currentWattage.Size = new System.Drawing.Size(75, 20);
      this.tbx_currentWattage.TabIndex = 18;
      // 
      // tb_temperature
      // 
      this.tb_temperature.LargeChange = 10;
      this.tb_temperature.Location = new System.Drawing.Point(256, 38);
      this.tb_temperature.Name = "tb_temperature";
      this.tb_temperature.Size = new System.Drawing.Size(190, 45);
      this.tb_temperature.TabIndex = 17;
      this.tb_temperature.ValueChanged += new System.EventHandler(this.tb_temperature_ValueChanged);
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(253, 22);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(61, 13);
      this.label7.TabIndex = 16;
      this.label7.Text = "Temperatur";
      // 
      // PlantSimulator
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(755, 557);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.tbxPassword);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.tbxPlantId);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.cmbxLogFormat);
      this.Controls.Add(this.cmbxInverterCount);
      this.Controls.Add(this.tbxLog);
      this.Controls.Add(this.btnStop);
      this.Controls.Add(this.btnStart);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.tbxUrl);
      this.MinimumSize = new System.Drawing.Size(750, 300);
      this.Name = "PlantSimulator";
      this.Text = "PV Simulator";
      ((System.ComponentModel.ISupportInitialize)(this.tb_wattage)).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.tb_temperature)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox tbxLog;
        private System.Windows.Forms.ComboBox cmbxInverterCount;
        private System.Windows.Forms.ComboBox cmbxLogFormat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxPlantId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar tb_wattage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar tb_temperature;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbx_temperature;
        private System.Windows.Forms.TextBox tbx_currentWattage;
    }
}

