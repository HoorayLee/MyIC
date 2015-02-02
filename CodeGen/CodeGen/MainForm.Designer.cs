namespace CodeGen
{
    partial class MainForm
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tb_connStr = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_tables = new System.Windows.Forms.ComboBox();
            this.btn_gen = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_model = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tb_DAL = new System.Windows.Forms.TextBox();
            this.tb_namespace = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_genAll = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb_connStr
            // 
            this.tb_connStr.Location = new System.Drawing.Point(119, 10);
            this.tb_connStr.Name = "tb_connStr";
            this.tb_connStr.Size = new System.Drawing.Size(408, 21);
            this.tb_connStr.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(533, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "连接";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "连接数据库字符串";
            // 
            // cb_tables
            // 
            this.cb_tables.Enabled = false;
            this.cb_tables.FormattingEnabled = true;
            this.cb_tables.Location = new System.Drawing.Point(119, 37);
            this.cb_tables.Name = "cb_tables";
            this.cb_tables.Size = new System.Drawing.Size(121, 20);
            this.cb_tables.TabIndex = 3;
            // 
            // btn_gen
            // 
            this.btn_gen.Enabled = false;
            this.btn_gen.Location = new System.Drawing.Point(452, 35);
            this.btn_gen.Name = "btn_gen";
            this.btn_gen.Size = new System.Drawing.Size(75, 23);
            this.btn_gen.TabIndex = 1;
            this.btn_gen.Text = "生成";
            this.btn_gen.UseVisualStyleBackColor = true;
            this.btn_gen.Click += new System.EventHandler(this.btn_gen_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "数据库表";
            // 
            // tb_model
            // 
            this.tb_model.Location = new System.Drawing.Point(6, 20);
            this.tb_model.Multiline = true;
            this.tb_model.Name = "tb_model";
            this.tb_model.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_model.Size = new System.Drawing.Size(300, 350);
            this.tb_model.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tb_DAL);
            this.groupBox1.Controls.Add(this.tb_model);
            this.groupBox1.Location = new System.Drawing.Point(14, 63);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(626, 375);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "生成代码";
            // 
            // tb_DAL
            // 
            this.tb_DAL.Location = new System.Drawing.Point(320, 20);
            this.tb_DAL.Multiline = true;
            this.tb_DAL.Name = "tb_DAL";
            this.tb_DAL.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_DAL.Size = new System.Drawing.Size(300, 350);
            this.tb_DAL.TabIndex = 5;
            // 
            // tb_namespace
            // 
            this.tb_namespace.Location = new System.Drawing.Point(315, 35);
            this.tb_namespace.Name = "tb_namespace";
            this.tb_namespace.Size = new System.Drawing.Size(124, 21);
            this.tb_namespace.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(256, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "命名空间";
            // 
            // btn_genAll
            // 
            this.btn_genAll.Enabled = false;
            this.btn_genAll.Location = new System.Drawing.Point(533, 35);
            this.btn_genAll.Name = "btn_genAll";
            this.btn_genAll.Size = new System.Drawing.Size(75, 23);
            this.btn_genAll.TabIndex = 1;
            this.btn_genAll.Text = "生成所有";
            this.btn_genAll.UseVisualStyleBackColor = true;
            this.btn_genAll.Click += new System.EventHandler(this.btn_genAll_Click);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "选择存放的文件夹";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 450);
            this.Controls.Add(this.tb_namespace);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cb_tables);
            this.Controls.Add(this.btn_genAll);
            this.Controls.Add(this.btn_gen);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tb_connStr);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "代码生成器";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_connStr;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb_tables;
        private System.Windows.Forms.Button btn_gen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_model;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_DAL;
        private System.Windows.Forms.TextBox tb_namespace;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_genAll;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}

