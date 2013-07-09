namespace File_Renamer
{
  partial class TestDataApp
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
        this.dtp_Source = new System.Windows.Forms.DateTimePicker();
        this.btn_createData = new System.Windows.Forms.Button();
        this.lbx_target = new System.Windows.Forms.ListBox();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.groupBox2 = new System.Windows.Forms.GroupBox();
        this.dtp_target_From = new System.Windows.Forms.DateTimePicker();
        this.dtp_Target_To = new System.Windows.Forms.DateTimePicker();
        this.label1 = new System.Windows.Forms.Label();
        this.label2 = new System.Windows.Forms.Label();
        this.groupBox1.SuspendLayout();
        this.groupBox2.SuspendLayout();
        this.SuspendLayout();
        // 
        // dtp_Source
        // 
        this.dtp_Source.Location = new System.Drawing.Point(6, 19);
        this.dtp_Source.Name = "dtp_Source";
        this.dtp_Source.Size = new System.Drawing.Size(200, 20);
        this.dtp_Source.TabIndex = 0;
        // 
        // btn_createData
        // 
        this.btn_createData.Location = new System.Drawing.Point(12, 189);
        this.btn_createData.Name = "btn_createData";
        this.btn_createData.Size = new System.Drawing.Size(215, 23);
        this.btn_createData.TabIndex = 1;
        this.btn_createData.Text = "Duplicate Test Day";
        this.btn_createData.UseVisualStyleBackColor = true;
        this.btn_createData.Click += new System.EventHandler(this.btn_createData_Click);
        // 
        // lbx_target
        // 
        this.lbx_target.FormattingEnabled = true;
        this.lbx_target.Items.AddRange(new object[] {
            "Development",
            "Test"});
        this.lbx_target.Location = new System.Drawing.Point(6, 19);
        this.lbx_target.Name = "lbx_target";
        this.lbx_target.Size = new System.Drawing.Size(119, 30);
        this.lbx_target.TabIndex = 2;
        // 
        // groupBox1
        // 
        this.groupBox1.Controls.Add(this.dtp_Source);
        this.groupBox1.Location = new System.Drawing.Point(12, 12);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(215, 71);
        this.groupBox1.TabIndex = 3;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "Source";
        // 
        // groupBox2
        // 
        this.groupBox2.Controls.Add(this.label2);
        this.groupBox2.Controls.Add(this.label1);
        this.groupBox2.Controls.Add(this.dtp_Target_To);
        this.groupBox2.Controls.Add(this.dtp_target_From);
        this.groupBox2.Controls.Add(this.lbx_target);
        this.groupBox2.Location = new System.Drawing.Point(300, 12);
        this.groupBox2.Name = "groupBox2";
        this.groupBox2.Size = new System.Drawing.Size(281, 149);
        this.groupBox2.TabIndex = 4;
        this.groupBox2.TabStop = false;
        this.groupBox2.Text = "Target";
        // 
        // dtp_target_From
        // 
        this.dtp_target_From.Location = new System.Drawing.Point(6, 86);
        this.dtp_target_From.Name = "dtp_target_From";
        this.dtp_target_From.Size = new System.Drawing.Size(200, 20);
        this.dtp_target_From.TabIndex = 3;
        // 
        // dtp_Target_To
        // 
        this.dtp_Target_To.Location = new System.Drawing.Point(6, 123);
        this.dtp_Target_To.Name = "dtp_Target_To";
        this.dtp_Target_To.Size = new System.Drawing.Size(200, 20);
        this.dtp_Target_To.TabIndex = 4;
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(3, 70);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(26, 13);
        this.label1.TabIndex = 5;
        this.label1.Text = "Von";
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Location = new System.Drawing.Point(3, 107);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(21, 13);
        this.label2.TabIndex = 6;
        this.label2.Text = "Bis";
        // 
        // Renamer
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(858, 224);
        this.Controls.Add(this.groupBox2);
        this.Controls.Add(this.groupBox1);
        this.Controls.Add(this.btn_createData);
        this.Name = "Renamer";
        this.Text = "Renamer";
        this.groupBox1.ResumeLayout(false);
        this.groupBox2.ResumeLayout(false);
        this.groupBox2.PerformLayout();
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DateTimePicker dtp_Source;
    private System.Windows.Forms.Button btn_createData;
    private System.Windows.Forms.ListBox lbx_target;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.DateTimePicker dtp_Target_To;
    private System.Windows.Forms.DateTimePicker dtp_target_From;

  }
}

