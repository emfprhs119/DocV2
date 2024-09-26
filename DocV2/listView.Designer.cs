namespace DocV2
{
    partial class ListView
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.reoGrid = new unvell.ReoGrid.ReoGridControl();
            this.panel1.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Location = new System.Drawing.Point(498, 4);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 36);
            this.button1.TabIndex = 1;
            this.button1.Text = "불러오기";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Right;
            this.button2.Location = new System.Drawing.Point(403, 4);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 36);
            this.button2.TabIndex = 2;
            this.button2.Text = "제거";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // searchBox
            // 
            this.searchBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchBox.Font = new System.Drawing.Font("Malgun Gothic", 12F);
            this.searchBox.Location = new System.Drawing.Point(0, 0);
            this.searchBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(593, 34);
            this.searchBox.TabIndex = 3;
            this.searchBox.TextChanged += new System.EventHandler(this.searchBox_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.searchBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.panel1.Size = new System.Drawing.Size(593, 40);
            this.panel1.TabIndex = 5;
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.button2);
            this.bottomPanel.Controls.Add(this.button1);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(3, 430);
            this.bottomPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.bottomPanel.Size = new System.Drawing.Size(593, 40);
            this.bottomPanel.TabIndex = 6;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.reoGrid);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 44);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(593, 386);
            this.panel3.TabIndex = 7;
            // 
            // reoGrid
            // 
            this.reoGrid.BackColor = System.Drawing.Color.White;
            this.reoGrid.ColumnHeaderContextMenuStrip = null;
            this.reoGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGrid.LeadHeaderContextMenuStrip = null;
            this.reoGrid.Location = new System.Drawing.Point(0, 0);
            this.reoGrid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.reoGrid.Name = "reoGrid";
            this.reoGrid.RowHeaderContextMenuStrip = null;
            this.reoGrid.Script = null;
            this.reoGrid.SheetTabContextMenuStrip = null;
            this.reoGrid.SheetTabNewButtonVisible = true;
            this.reoGrid.SheetTabVisible = true;
            this.reoGrid.SheetTabWidth = 69;
            this.reoGrid.ShowScrollEndSpacing = true;
            this.reoGrid.Size = new System.Drawing.Size(591, 384);
            this.reoGrid.TabIndex = 1;
            this.reoGrid.Text = "reoGridControl1";
            // 
            // ListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(599, 474);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ListView";
            this.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ShowIcon = false;
            this.Text = "View";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoadDoc_FormClosing);
            this.Load += new System.EventHandler(this.LoadDoc_Load);
            this.Resize += new System.EventHandler(this.DocView_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Panel panel3;
        private unvell.ReoGrid.ReoGridControl reoGrid;
    }
}