namespace DocV2
{
    partial class PdfView
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
            this.tableFontCombo = new System.Windows.Forms.ComboBox();
            this.tableSizeCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.leftLayout = new System.Windows.Forms.Panel();
            this.accept = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.headerSizeCombo = new System.Windows.Forms.ComboBox();
            this.headerFontCombo = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.leftLayout.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableFontCombo
            // 
            this.tableFontCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableFontCombo.Font = new System.Drawing.Font("굴림", 12F);
            this.tableFontCombo.FormattingEnabled = true;
            this.tableFontCombo.Location = new System.Drawing.Point(56, 25);
            this.tableFontCombo.Name = "tableFontCombo";
            this.tableFontCombo.Size = new System.Drawing.Size(226, 24);
            this.tableFontCombo.TabIndex = 0;
            // 
            // tableSizeCombo
            // 
            this.tableSizeCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableSizeCombo.Font = new System.Drawing.Font("굴림", 12F);
            this.tableSizeCombo.FormattingEnabled = true;
            this.tableSizeCombo.Location = new System.Drawing.Point(56, 55);
            this.tableSizeCombo.Name = "tableSizeCombo";
            this.tableSizeCombo.Size = new System.Drawing.Size(226, 24);
            this.tableSizeCombo.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 12F);
            this.label1.Location = new System.Drawing.Point(0, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "폰트";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 12F);
            this.label2.Location = new System.Drawing.Point(-1, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "크기";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(309, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(872, 755);
            this.panel2.TabIndex = 4;
            // 
            // leftLayout
            // 
            this.leftLayout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.leftLayout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.leftLayout.Controls.Add(this.accept);
            this.leftLayout.Controls.Add(this.groupBox2);
            this.leftLayout.Controls.Add(this.groupBox1);
            this.leftLayout.Location = new System.Drawing.Point(3, 3);
            this.leftLayout.Name = "leftLayout";
            this.leftLayout.Size = new System.Drawing.Size(300, 755);
            this.leftLayout.TabIndex = 5;
            // 
            // accept
            // 
            this.accept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.accept.Location = new System.Drawing.Point(228, 198);
            this.accept.Name = "accept";
            this.accept.Size = new System.Drawing.Size(63, 23);
            this.accept.TabIndex = 6;
            this.accept.Text = "적용";
            this.accept.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.headerSizeCombo);
            this.groupBox2.Controls.Add(this.headerFontCombo);
            this.groupBox2.Font = new System.Drawing.Font("굴림", 12F);
            this.groupBox2.Location = new System.Drawing.Point(3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(288, 92);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "기본정보";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 12F);
            this.label3.Location = new System.Drawing.Point(0, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "폰트";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 12F);
            this.label4.Location = new System.Drawing.Point(-1, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "크기";
            // 
            // headerSizeCombo
            // 
            this.headerSizeCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headerSizeCombo.Font = new System.Drawing.Font("굴림", 12F);
            this.headerSizeCombo.FormattingEnabled = true;
            this.headerSizeCombo.Location = new System.Drawing.Point(56, 55);
            this.headerSizeCombo.Name = "headerSizeCombo";
            this.headerSizeCombo.Size = new System.Drawing.Size(226, 24);
            this.headerSizeCombo.TabIndex = 1;
            // 
            // headerFontCombo
            // 
            this.headerFontCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headerFontCombo.Font = new System.Drawing.Font("굴림", 12F);
            this.headerFontCombo.FormattingEnabled = true;
            this.headerFontCombo.Location = new System.Drawing.Point(56, 25);
            this.headerFontCombo.Name = "headerFontCombo";
            this.headerFontCombo.Size = new System.Drawing.Size(226, 24);
            this.headerFontCombo.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tableSizeCombo);
            this.groupBox1.Controls.Add(this.tableFontCombo);
            this.groupBox1.Font = new System.Drawing.Font("굴림", 12F);
            this.groupBox1.Location = new System.Drawing.Point(3, 102);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(288, 92);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "테이블";
            // 
            // PdfView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1184, 761);
            this.Controls.Add(this.leftLayout);
            this.Controls.Add(this.panel2);
            this.Name = "PdfView";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "ExportPDF_Config";
            this.leftLayout.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox tableFontCombo;
        private System.Windows.Forms.ComboBox tableSizeCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel leftLayout;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox headerSizeCombo;
        private System.Windows.Forms.ComboBox headerFontCombo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button accept;
    }
}