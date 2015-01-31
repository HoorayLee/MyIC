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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("今天", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("昨天", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("两天前", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "2014-12-12 13:00:00",
            "Tony",
            "牙龈出血"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "2014-12-11 11:00:00",
            "Sakura",
            "？？？"}, -1);
            this.lv_selfCheck = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lv_selfCheck
            // 
            this.lv_selfCheck.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lv_selfCheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_selfCheck.FullRowSelect = true;
            listViewGroup1.Header = "今天";
            listViewGroup1.Name = "listViewGroup1";
            listViewGroup2.Header = "昨天";
            listViewGroup2.Name = "listViewGroup2";
            listViewGroup3.Header = "两天前";
            listViewGroup3.Name = "listViewGroup3";
            this.lv_selfCheck.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            listViewItem1.Group = listViewGroup1;
            listViewItem2.Group = listViewGroup2;
            this.lv_selfCheck.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.lv_selfCheck.Location = new System.Drawing.Point(0, 0);
            this.lv_selfCheck.MultiSelect = false;
            this.lv_selfCheck.Name = "lv_selfCheck";
            this.lv_selfCheck.Size = new System.Drawing.Size(434, 297);
            this.lv_selfCheck.TabIndex = 3;
            this.lv_selfCheck.UseCompatibleStateImageBehavior = false;
            this.lv_selfCheck.View = System.Windows.Forms.View.Details;
            this.lv_selfCheck.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lv_selfCheck_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "提交时间";
            this.columnHeader1.Width = 145;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "用户名";
            this.columnHeader2.Width = 69;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "描述";
            this.columnHeader3.Width = 187;
            // 
            // SelfCheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 297);
            this.Controls.Add(this.lv_selfCheck);
            this.Name = "SelfCheckForm";
            this.Text = "自检查看";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lv_selfCheck;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;

    }
}