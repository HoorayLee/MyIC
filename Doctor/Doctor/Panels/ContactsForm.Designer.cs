namespace Doctor.Panels
{
    partial class ContactsForm
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Tony",
            "123",
            "aa"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Sakura",
            "Hello"}, -1);
            this.lv_contacts = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lv_contacts
            // 
            this.lv_contacts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lv_contacts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_contacts.FullRowSelect = true;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.StateImageIndex = 0;
            this.lv_contacts.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.lv_contacts.LabelWrap = false;
            this.lv_contacts.Location = new System.Drawing.Point(0, 0);
            this.lv_contacts.MultiSelect = false;
            this.lv_contacts.Name = "lv_contacts";
            this.lv_contacts.Size = new System.Drawing.Size(437, 297);
            this.lv_contacts.TabIndex = 0;
            this.lv_contacts.UseCompatibleStateImageBehavior = false;
            this.lv_contacts.View = System.Windows.Forms.View.Details;
            this.lv_contacts.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lv_contacts_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "用户名";
            this.columnHeader1.Width = 96;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "最近信息";
            this.columnHeader2.Width = 288;
            // 
            // ContactsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 297);
            this.Controls.Add(this.lv_contacts);
            this.Name = "ContactsForm";
            this.Text = "联系人";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lv_contacts;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;


    }
}