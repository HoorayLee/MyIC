namespace Doctor.Panels
{
    partial class SelfCheckForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.记录编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.提交日期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.用户编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.描述 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.记录编号,
            this.提交日期,
            this.用户编号,
            this.描述});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(475, 297);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            // 
            // 记录编号
            // 
            this.记录编号.DataPropertyName = "Record_id";
            this.记录编号.HeaderText = "记录编号";
            this.记录编号.Name = "记录编号";
            this.记录编号.ReadOnly = true;
            this.记录编号.Width = 80;
            // 
            // 提交日期
            // 
            this.提交日期.DataPropertyName = "Time";
            this.提交日期.HeaderText = "提交日期";
            this.提交日期.Name = "提交日期";
            this.提交日期.ReadOnly = true;
            // 
            // 用户编号
            // 
            this.用户编号.DataPropertyName = "User_id";
            this.用户编号.HeaderText = "用户编号";
            this.用户编号.Name = "用户编号";
            this.用户编号.ReadOnly = true;
            // 
            // 描述
            // 
            this.描述.DataPropertyName = "Description";
            this.描述.HeaderText = "描述";
            this.描述.Name = "描述";
            this.描述.ReadOnly = true;
            this.描述.Width = 150;
            // 
            // SelfCheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 297);
            this.Controls.Add(this.dataGridView1);
            this.Name = "SelfCheckForm";
            this.Text = "自检查看";
            this.Load += new System.EventHandler(this.SelfCheckForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 记录编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 提交日期;
        private System.Windows.Forms.DataGridViewTextBoxColumn 用户编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 描述;


    }
}