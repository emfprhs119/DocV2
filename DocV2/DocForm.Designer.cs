namespace DocV2
{
    partial class DocForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.loadCompanyBtn = new System.Windows.Forms.Button();
            this.t4 = new System.Windows.Forms.TextBox();
            this.t3 = new System.Windows.Forms.TextBox();
            this.t2 = new System.Windows.Forms.TextBox();
            this.o4 = new System.Windows.Forms.Label();
            this.o3 = new System.Windows.Forms.Label();
            this.o2 = new System.Windows.Forms.Label();
            this.o1 = new System.Windows.Forms.Label();
            this.t1 = new System.Windows.Forms.DateTimePicker();
            this.panel2 = new System.Windows.Forms.Panel();
            this.s8 = new System.Windows.Forms.Label();
            this.s6 = new System.Windows.Forms.Label();
            this.s3 = new System.Windows.Forms.Label();
            this.s7 = new System.Windows.Forms.Label();
            this.s5 = new System.Windows.Forms.Label();
            this.s4 = new System.Windows.Forms.Label();
            this.s2 = new System.Windows.Forms.Label();
            this.s1 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.headLabel = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.b3 = new System.Windows.Forms.Button();
            this.b2 = new System.Windows.Forms.Button();
            this.b1 = new System.Windows.Forms.Button();
            this.reoGrid = new unvell.ReoGrid.ReoGridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menu_RowAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_RowDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_DataRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menu_Cut = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_Paste = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label24);
            this.panel1.Controls.Add(this.label23);
            this.panel1.Controls.Add(this.loadCompanyBtn);
            this.panel1.Controls.Add(this.t4);
            this.panel1.Controls.Add(this.t3);
            this.panel1.Controls.Add(this.t2);
            this.panel1.Controls.Add(this.o4);
            this.panel1.Controls.Add(this.o3);
            this.panel1.Controls.Add(this.o2);
            this.panel1.Controls.Add(this.o1);
            this.panel1.Controls.Add(this.t1);
            this.panel1.Location = new System.Drawing.Point(12, 86);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(330, 179);
            this.panel1.TabIndex = 1;
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label24.Location = new System.Drawing.Point(-1, 140);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(108, 32);
            this.label24.TabIndex = 11;
            this.label24.Text = "합계금액";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label23
            // 
            this.label23.BackColor = System.Drawing.SystemColors.Info;
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label23.Location = new System.Drawing.Point(113, 139);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(212, 33);
            this.label23.TabIndex = 10;
            this.label23.Text = "9,999,999,999원";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // loadCompanyBtn
            // 
            this.loadCompanyBtn.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.loadCompanyBtn.Location = new System.Drawing.Point(270, 37);
            this.loadCompanyBtn.Name = "loadCompanyBtn";
            this.loadCompanyBtn.Size = new System.Drawing.Size(30, 30);
            this.loadCompanyBtn.TabIndex = 9;
            this.loadCompanyBtn.Text = "...";
            this.loadCompanyBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.loadCompanyBtn.UseVisualStyleBackColor = true;
            this.loadCompanyBtn.Click += new System.EventHandler(this.loadCompanyBtn_Click);
            // 
            // t4
            // 
            this.t4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.t4.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.t4.Location = new System.Drawing.Point(113, 102);
            this.t4.Name = "t4";
            this.t4.Size = new System.Drawing.Size(187, 29);
            this.t4.TabIndex = 8;
            // 
            // t3
            // 
            this.t3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.t3.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.t3.Location = new System.Drawing.Point(113, 70);
            this.t3.Name = "t3";
            this.t3.Size = new System.Drawing.Size(187, 29);
            this.t3.TabIndex = 7;
            // 
            // t2
            // 
            this.t2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.t2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.t2.Location = new System.Drawing.Point(113, 38);
            this.t2.Name = "t2";
            this.t2.Size = new System.Drawing.Size(187, 29);
            this.t2.TabIndex = 6;
            // 
            // o4
            // 
            this.o4.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.o4.Location = new System.Drawing.Point(1, 101);
            this.o4.Name = "o4";
            this.o4.Size = new System.Drawing.Size(106, 32);
            this.o4.TabIndex = 5;
            this.o4.Text = "담당자";
            this.o4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // o3
            // 
            this.o3.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.o3.Location = new System.Drawing.Point(1, 69);
            this.o3.Name = "o3";
            this.o3.Size = new System.Drawing.Size(106, 32);
            this.o3.TabIndex = 4;
            this.o3.Text = "전화번호";
            this.o3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // o2
            // 
            this.o2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.o2.Location = new System.Drawing.Point(1, 37);
            this.o2.Name = "o2";
            this.o2.Size = new System.Drawing.Size(106, 32);
            this.o2.TabIndex = 3;
            this.o2.Text = "상 호";
            this.o2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // o1
            // 
            this.o1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.o1.Location = new System.Drawing.Point(1, 5);
            this.o1.Name = "o1";
            this.o1.Size = new System.Drawing.Size(106, 32);
            this.o1.TabIndex = 2;
            this.o1.Text = "견적일";
            this.o1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // t1
            // 
            this.t1.CalendarFont = new System.Drawing.Font("맑은 고딕", 12F);
            this.t1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.t1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.t1.Location = new System.Drawing.Point(113, 7);
            this.t1.Name = "t1";
            this.t1.Size = new System.Drawing.Size(187, 29);
            this.t1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.Controls.Add(this.s8);
            this.panel2.Controls.Add(this.s6);
            this.panel2.Controls.Add(this.s3);
            this.panel2.Controls.Add(this.s7);
            this.panel2.Controls.Add(this.s5);
            this.panel2.Controls.Add(this.s4);
            this.panel2.Controls.Add(this.s2);
            this.panel2.Controls.Add(this.s1);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Location = new System.Drawing.Point(348, 55);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(424, 210);
            this.panel2.TabIndex = 2;
            // 
            // s8
            // 
            this.s8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.s8.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.s8.Location = new System.Drawing.Point(273, 170);
            this.s8.Margin = new System.Windows.Forms.Padding(0);
            this.s8.Name = "s8";
            this.s8.Size = new System.Drawing.Size(151, 40);
            this.s8.TabIndex = 23;
            this.s8.Text = "label22";
            this.s8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.s8.DoubleClick += new System.EventHandler(this.UserField_DoubleClick);
            // 
            // s6
            // 
            this.s6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.s6.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.s6.Location = new System.Drawing.Point(273, 130);
            this.s6.Margin = new System.Windows.Forms.Padding(0);
            this.s6.Name = "s6";
            this.s6.Size = new System.Drawing.Size(151, 40);
            this.s6.TabIndex = 22;
            this.s6.Text = "label21";
            this.s6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.s6.DoubleClick += new System.EventHandler(this.UserField_DoubleClick);
            // 
            // s3
            // 
            this.s3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.s3.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.s3.Location = new System.Drawing.Point(273, 50);
            this.s3.Margin = new System.Windows.Forms.Padding(0);
            this.s3.Name = "s3";
            this.s3.Size = new System.Drawing.Size(151, 40);
            this.s3.TabIndex = 21;
            this.s3.Text = "label20";
            this.s3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.s3.DoubleClick += new System.EventHandler(this.UserField_DoubleClick);
            // 
            // s7
            // 
            this.s7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.s7.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.s7.Location = new System.Drawing.Point(77, 170);
            this.s7.Margin = new System.Windows.Forms.Padding(0);
            this.s7.Name = "s7";
            this.s7.Size = new System.Drawing.Size(151, 40);
            this.s7.TabIndex = 20;
            this.s7.Text = "label19";
            this.s7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.s7.DoubleClick += new System.EventHandler(this.UserField_DoubleClick);
            // 
            // s5
            // 
            this.s5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.s5.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.s5.Location = new System.Drawing.Point(77, 130);
            this.s5.Margin = new System.Windows.Forms.Padding(0);
            this.s5.Name = "s5";
            this.s5.Size = new System.Drawing.Size(151, 40);
            this.s5.TabIndex = 19;
            this.s5.Text = "label18";
            this.s5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.s5.DoubleClick += new System.EventHandler(this.UserField_DoubleClick);
            // 
            // s4
            // 
            this.s4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.s4.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.s4.Location = new System.Drawing.Point(77, 90);
            this.s4.Margin = new System.Windows.Forms.Padding(0);
            this.s4.Name = "s4";
            this.s4.Size = new System.Drawing.Size(347, 40);
            this.s4.TabIndex = 18;
            this.s4.Text = "label17";
            this.s4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.s4.DoubleClick += new System.EventHandler(this.UserField_DoubleClick);
            // 
            // s2
            // 
            this.s2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.s2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.s2.Location = new System.Drawing.Point(77, 50);
            this.s2.Margin = new System.Windows.Forms.Padding(0);
            this.s2.Name = "s2";
            this.s2.Size = new System.Drawing.Size(151, 40);
            this.s2.TabIndex = 17;
            this.s2.Text = "label16";
            this.s2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.s2.DoubleClick += new System.EventHandler(this.UserField_DoubleClick);
            // 
            // s1
            // 
            this.s1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.s1.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.s1.Location = new System.Drawing.Point(77, 0);
            this.s1.Margin = new System.Windows.Forms.Padding(0);
            this.s1.Name = "s1";
            this.s1.Size = new System.Drawing.Size(347, 50);
            this.s1.TabIndex = 10;
            this.s1.Text = "label15";
            this.s1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.s1.DoubleClick += new System.EventHandler(this.UserField_DoubleClick);
            // 
            // label12
            // 
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(228, 170);
            this.label12.Margin = new System.Windows.Forms.Padding(0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(45, 40);
            this.label12.TabIndex = 8;
            this.label12.Text = "팩스";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(228, 130);
            this.label13.Margin = new System.Windows.Forms.Padding(0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(45, 40);
            this.label13.TabIndex = 7;
            this.label13.Text = "종목";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.Location = new System.Drawing.Point(228, 50);
            this.label14.Margin = new System.Windows.Forms.Padding(0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(45, 40);
            this.label14.TabIndex = 6;
            this.label14.Text = "성명";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(32, 170);
            this.label11.Margin = new System.Windows.Forms.Padding(0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(45, 40);
            this.label11.TabIndex = 5;
            this.label11.Text = "전화";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(32, 130);
            this.label10.Margin = new System.Windows.Forms.Padding(0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(45, 40);
            this.label10.TabIndex = 4;
            this.label10.Text = "업태";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(32, 90);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 40);
            this.label9.TabIndex = 3;
            this.label9.Text = "주소";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(32, 50);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 40);
            this.label8.TabIndex = 2;
            this.label8.Text = "상호";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(32, 0);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 50);
            this.label7.TabIndex = 1;
            this.label7.Text = "등록번호";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 210);
            this.label6.TabIndex = 0;
            this.label6.Text = "공급자";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // headLabel
            // 
            this.headLabel.BackColor = System.Drawing.SystemColors.Window;
            this.headLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.headLabel.Font = new System.Drawing.Font("맑은 고딕", 35F);
            this.headLabel.Location = new System.Drawing.Point(12, 8);
            this.headLabel.Name = "headLabel";
            this.headLabel.Size = new System.Drawing.Size(330, 75);
            this.headLabel.TabIndex = 0;
            this.headLabel.Text = "견적서";
            this.headLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.Controls.Add(this.b3);
            this.panel4.Controls.Add(this.b2);
            this.panel4.Controls.Add(this.b1);
            this.panel4.Location = new System.Drawing.Point(348, 8);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(424, 44);
            this.panel4.TabIndex = 4;
            // 
            // b3
            // 
            this.b3.BackColor = System.Drawing.SystemColors.Window;
            this.b3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.b3.Location = new System.Drawing.Point(153, 0);
            this.b3.Name = "b3";
            this.b3.Size = new System.Drawing.Size(75, 44);
            this.b3.TabIndex = 2;
            this.b3.Text = "저장하기";
            this.b3.UseVisualStyleBackColor = false;
            this.b3.Click += new System.EventHandler(this.button3_Click);
            // 
            // b2
            // 
            this.b2.BackColor = System.Drawing.SystemColors.Window;
            this.b2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.b2.Location = new System.Drawing.Point(77, 0);
            this.b2.Name = "b2";
            this.b2.Size = new System.Drawing.Size(75, 44);
            this.b2.TabIndex = 1;
            this.b2.Text = "불러오기";
            this.b2.UseVisualStyleBackColor = false;
            this.b2.Click += new System.EventHandler(this.b2_Click);
            // 
            // b1
            // 
            this.b1.BackColor = System.Drawing.SystemColors.Window;
            this.b1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.b1.Location = new System.Drawing.Point(0, 0);
            this.b1.Name = "b1";
            this.b1.Size = new System.Drawing.Size(75, 44);
            this.b1.TabIndex = 0;
            this.b1.Text = "새 견적서";
            this.b1.UseVisualStyleBackColor = false;
            this.b1.Click += new System.EventHandler(this.b1_Click);
            // 
            // reoGrid
            // 
            this.reoGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reoGrid.BackColor = System.Drawing.Color.White;
            this.reoGrid.ColumnHeaderContextMenuStrip = null;
            this.reoGrid.LeadHeaderContextMenuStrip = null;
            this.reoGrid.Location = new System.Drawing.Point(-1, -1);
            this.reoGrid.Name = "reoGrid";
            this.reoGrid.Padding = new System.Windows.Forms.Padding(100);
            this.reoGrid.RowHeaderContextMenuStrip = this.contextMenuStrip1;
            this.reoGrid.Script = null;
            this.reoGrid.SheetTabContextMenuStrip = null;
            this.reoGrid.SheetTabNewButtonVisible = true;
            this.reoGrid.SheetTabVisible = true;
            this.reoGrid.SheetTabWidth = 60;
            this.reoGrid.ShowScrollEndSpacing = true;
            this.reoGrid.Size = new System.Drawing.Size(760, 455);
            this.reoGrid.TabIndex = 7;
            this.reoGrid.Text = "reoGridControl1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_RowAdd,
            this.menu_RowDelete,
            this.menu_DataRemove,
            this.toolStripSeparator1,
            this.menu_Cut,
            this.menu_Copy,
            this.menu_Paste});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(139, 142);
            // 
            // menu_RowAdd
            // 
            this.menu_RowAdd.Name = "menu_RowAdd";
            this.menu_RowAdd.Size = new System.Drawing.Size(138, 22);
            this.menu_RowAdd.Text = "행 삽입";
            // 
            // menu_RowDelete
            // 
            this.menu_RowDelete.Name = "menu_RowDelete";
            this.menu_RowDelete.Size = new System.Drawing.Size(138, 22);
            this.menu_RowDelete.Text = "행 삭제";
            // 
            // menu_DataRemove
            // 
            this.menu_DataRemove.Name = "menu_DataRemove";
            this.menu_DataRemove.Size = new System.Drawing.Size(138, 22);
            this.menu_DataRemove.Text = "내용 지우기";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(135, 6);
            // 
            // menu_Cut
            // 
            this.menu_Cut.Name = "menu_Cut";
            this.menu_Cut.Size = new System.Drawing.Size(138, 22);
            this.menu_Cut.Text = "잘라내기";
            // 
            // menu_Copy
            // 
            this.menu_Copy.Name = "menu_Copy";
            this.menu_Copy.Size = new System.Drawing.Size(138, 22);
            this.menu_Copy.Text = "복사";
            // 
            // menu_Paste
            // 
            this.menu_Paste.Name = "menu_Paste";
            this.menu_Paste.Size = new System.Drawing.Size(138, 22);
            this.menu_Paste.Text = "붙여넣기";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.reoGrid);
            this.panel3.Location = new System.Drawing.Point(12, 271);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(760, 455);
            this.panel3.TabIndex = 8;
            // 
            // DocForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 738);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.headLabel);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "DocForm";
            this.Text = "견적서";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DocForm_FormClosing);
            this.Load += new System.EventHandler(this.DocForm_Load);
            this.Resize += new System.EventHandler(this.DocForm_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label headLabel;
        private System.Windows.Forms.TextBox t4;
        private System.Windows.Forms.TextBox t3;
        private System.Windows.Forms.TextBox t2;
        private System.Windows.Forms.Label o4;
        private System.Windows.Forms.Label o3;
        private System.Windows.Forms.Label o2;
        private System.Windows.Forms.Label o1;
        private System.Windows.Forms.DateTimePicker t1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button b3;
        private System.Windows.Forms.Button b2;
        private System.Windows.Forms.Button b1;
        private System.Windows.Forms.Button loadCompanyBtn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label s8;
        private System.Windows.Forms.Label s6;
        private System.Windows.Forms.Label s3;
        private System.Windows.Forms.Label s7;
        private System.Windows.Forms.Label s5;
        private System.Windows.Forms.Label s4;
        private System.Windows.Forms.Label s2;
        private System.Windows.Forms.Label s1;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private unvell.ReoGrid.ReoGridControl reoGrid;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menu_RowAdd;
        private System.Windows.Forms.ToolStripMenuItem menu_RowDelete;
        private System.Windows.Forms.ToolStripMenuItem menu_DataRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menu_Cut;
        private System.Windows.Forms.ToolStripMenuItem menu_Copy;
        private System.Windows.Forms.ToolStripMenuItem menu_Paste;
        private System.Windows.Forms.Panel panel3;
    }
}

