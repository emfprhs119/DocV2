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
            this.button1.Location = new System.Drawing.Point(435, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(83, 29);
            this.button1.TabIndex = 1;
            this.button1.Text = "불러오기";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Right;
            this.button2.Location = new System.Drawing.Point(352, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(83, 29);
            this.button2.TabIndex = 2;
            this.button2.Text = "제거";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // searchBox
            // 
            this.searchBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchBox.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.searchBox.Location = new System.Drawing.Point(0, 0);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(518, 29);
            this.searchBox.TabIndex = 3;
            this.searchBox.TextChanged += new System.EventHandler(this.searchBox_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.searchBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.panel1.Size = new System.Drawing.Size(518, 32);
            this.panel1.TabIndex = 5;
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.button2);
            this.bottomPanel.Controls.Add(this.button1);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(3, 344);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.bottomPanel.Size = new System.Drawing.Size(518, 32);
            this.bottomPanel.TabIndex = 6;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.reoGrid);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 35);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(518, 309);
            this.panel3.TabIndex = 7;
            // 
            // reoGrid
            // 
            this.reoGrid.BackColor = System.Drawing.Color.White;
            this.reoGrid.ColumnHeaderContextMenuStrip = null;
            this.reoGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGrid.LeadHeaderContextMenuStrip = null;
            this.reoGrid.Location = new System.Drawing.Point(0, 0);
            this.reoGrid.Name = "reoGrid";
            this.reoGrid.RowHeaderContextMenuStrip = null;
            this.reoGrid.Script = null;
            this.reoGrid.SheetTabContextMenuStrip = null;
            this.reoGrid.SheetTabNewButtonVisible = true;
            this.reoGrid.SheetTabVisible = true;
            this.reoGrid.SheetTabWidth = 60;
            this.reoGrid.ShowScrollEndSpacing = true;
            this.reoGrid.Size = new System.Drawing.Size(516, 307);
            this.reoGrid.TabIndex = 1;
            this.reoGrid.Text = "reoGridControl1";
            // 
            // ListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(524, 379);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.panel1);
            this.Name = "ListView";
            this.Padding = new System.Windows.Forms.Padding(3);
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