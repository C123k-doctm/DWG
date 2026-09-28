namespace LicenceTool
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnBrowser = new Button();
            label1 = new Label();
            txtSourceFile = new TextBox();
            btnGenerate = new Button();
            rbtSeven = new RadioButton();
            label2 = new Label();
            rbtnTen = new RadioButton();
            rbtnFifteen = new RadioButton();
            rbtnThirty = new RadioButton();
            rbtnSixty = new RadioButton();
            rbtnNinety = new RadioButton();
            rbtnSixMonth = new RadioButton();
            rbtnForver = new RadioButton();
            label3 = new Label();
            txtPublicKey = new TextBox();
            label4 = new Label();
            txtPrivateKey = new TextBox();
            SuspendLayout();
            // 
            // btnBrowser
            // 
            btnBrowser.Location = new Point(537, 72);
            btnBrowser.Name = "btnBrowser";
            btnBrowser.Size = new Size(75, 23);
            btnBrowser.TabIndex = 0;
            btnBrowser.Text = "浏览...";
            btnBrowser.UseVisualStyleBackColor = true;
            btnBrowser.Click += btnBrowser_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(68, 75);
            label1.Name = "label1";
            label1.Size = new Size(83, 17);
            label1.TabIndex = 1;
            label1.Text = "客户信息文件:";
            // 
            // txtSourceFile
            // 
            txtSourceFile.Location = new Point(167, 72);
            txtSourceFile.Name = "txtSourceFile";
            txtSourceFile.Size = new Size(364, 23);
            txtSourceFile.TabIndex = 2;
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(628, 72);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(87, 23);
            btnGenerate.TabIndex = 3;
            btnGenerate.Text = "生成Licence";
            btnGenerate.UseVisualStyleBackColor = true;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // rbtSeven
            // 
            rbtSeven.AutoSize = true;
            rbtSeven.Checked = true;
            rbtSeven.Location = new Point(167, 36);
            rbtSeven.Name = "rbtSeven";
            rbtSeven.Size = new Size(45, 21);
            rbtSeven.TabIndex = 4;
            rbtSeven.TabStop = true;
            rbtSeven.Tag = "7";
            rbtSeven.Text = "7天";
            rbtSeven.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(68, 38);
            label2.Name = "label2";
            label2.Size = new Size(89, 17);
            label2.TabIndex = 5;
            label2.Text = "Licence有效期:";
            // 
            // rbtnTen
            // 
            rbtnTen.AutoSize = true;
            rbtnTen.Location = new Point(225, 36);
            rbtnTen.Name = "rbtnTen";
            rbtnTen.Size = new Size(52, 21);
            rbtnTen.TabIndex = 4;
            rbtnTen.Tag = "10";
            rbtnTen.Text = "10天";
            rbtnTen.UseVisualStyleBackColor = true;
            // 
            // rbtnFifteen
            // 
            rbtnFifteen.AutoSize = true;
            rbtnFifteen.Location = new Point(290, 36);
            rbtnFifteen.Name = "rbtnFifteen";
            rbtnFifteen.Size = new Size(52, 21);
            rbtnFifteen.TabIndex = 4;
            rbtnFifteen.Tag = "15";
            rbtnFifteen.Text = "15天";
            rbtnFifteen.UseVisualStyleBackColor = true;
            // 
            // rbtnThirty
            // 
            rbtnThirty.AutoSize = true;
            rbtnThirty.Location = new Point(355, 36);
            rbtnThirty.Name = "rbtnThirty";
            rbtnThirty.Size = new Size(52, 21);
            rbtnThirty.TabIndex = 4;
            rbtnThirty.Tag = "30";
            rbtnThirty.Text = "30天";
            rbtnThirty.UseVisualStyleBackColor = true;
            // 
            // rbtnSixty
            // 
            rbtnSixty.AutoSize = true;
            rbtnSixty.Location = new Point(420, 36);
            rbtnSixty.Name = "rbtnSixty";
            rbtnSixty.Size = new Size(52, 21);
            rbtnSixty.TabIndex = 4;
            rbtnSixty.Tag = "60";
            rbtnSixty.Text = "60天";
            rbtnSixty.UseVisualStyleBackColor = true;
            // 
            // rbtnNinety
            // 
            rbtnNinety.AutoSize = true;
            rbtnNinety.Location = new Point(485, 36);
            rbtnNinety.Name = "rbtnNinety";
            rbtnNinety.Size = new Size(52, 21);
            rbtnNinety.TabIndex = 4;
            rbtnNinety.Tag = "90";
            rbtnNinety.Text = "90天";
            rbtnNinety.UseVisualStyleBackColor = true;
            // 
            // rbtnSixMonth
            // 
            rbtnSixMonth.AutoSize = true;
            rbtnSixMonth.Location = new Point(550, 36);
            rbtnSixMonth.Name = "rbtnSixMonth";
            rbtnSixMonth.Size = new Size(59, 21);
            rbtnSixMonth.TabIndex = 4;
            rbtnSixMonth.Tag = "180";
            rbtnSixMonth.Text = "180天";
            rbtnSixMonth.UseVisualStyleBackColor = true;
            // 
            // rbtnForver
            // 
            rbtnForver.AutoSize = true;
            rbtnForver.Location = new Point(622, 36);
            rbtnForver.Name = "rbtnForver";
            rbtnForver.Size = new Size(50, 21);
            rbtnForver.TabIndex = 4;
            rbtnForver.Tag = "-1";
            rbtnForver.Text = "永久";
            rbtnForver.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(68, 109);
            label3.Name = "label3";
            label3.Size = new Size(68, 17);
            label3.TabIndex = 6;
            label3.Text = "公共秘钥：";
            // 
            // txtPublicKey
            // 
            txtPublicKey.Location = new Point(167, 106);
            txtPublicKey.Multiline = true;
            txtPublicKey.Name = "txtPublicKey";
            txtPublicKey.Size = new Size(364, 111);
            txtPublicKey.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(68, 235);
            label4.Name = "label4";
            label4.Size = new Size(68, 17);
            label4.TabIndex = 6;
            label4.Text = "私有秘钥：";
            // 
            // txtPrivateKey
            // 
            txtPrivateKey.Location = new Point(167, 232);
            txtPrivateKey.Multiline = true;
            txtPrivateKey.Name = "txtPrivateKey";
            txtPrivateKey.Size = new Size(364, 111);
            txtPrivateKey.TabIndex = 7;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtPrivateKey);
            Controls.Add(label4);
            Controls.Add(txtPublicKey);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(rbtnForver);
            Controls.Add(rbtnSixMonth);
            Controls.Add(rbtnNinety);
            Controls.Add(rbtnSixty);
            Controls.Add(rbtnThirty);
            Controls.Add(rbtnFifteen);
            Controls.Add(rbtnTen);
            Controls.Add(rbtSeven);
            Controls.Add(btnGenerate);
            Controls.Add(txtSourceFile);
            Controls.Add(label1);
            Controls.Add(btnBrowser);
            Name = "MainForm";
            Text = "Licence生成工具";
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnBrowser;
        private Label label1;
        private TextBox txtSourceFile;
        private Button btnGenerate;
        private RadioButton rbtSeven;
        private Label label2;
        private RadioButton rbtnTen;
        private RadioButton rbtnFifteen;
        private RadioButton rbtnThirty;
        private RadioButton rbtnSixty;
        private RadioButton rbtnNinety;
        private RadioButton rbtnSixMonth;
        private RadioButton rbtnForver;
        private Label label3;
        private TextBox txtPublicKey;
        private Label label4;
        private TextBox txtPrivateKey;
    }
}