namespace StudentManagement
{
    partial class UserManagementForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.grpAdd = new System.Windows.Forms.GroupBox();
            this.txtNewUsername = new System.Windows.Forms.TextBox();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.lblNewUsername = new System.Windows.Forms.Label();
            this.lblNewPassword = new System.Windows.Forms.Label();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.btnDeleteUser = new System.Windows.Forms.Button();
            this.grpPassword = new System.Windows.Forms.GroupBox();
            this.lblChangePassword = new System.Windows.Forms.Label();
            this.txtChangePassword = new System.Windows.Forms.TextBox();
            this.btnChangePassword = new System.Windows.Forms.Button();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.grpAdd.SuspendLayout();
            this.grpPassword.SuspendLayout();
            this.SuspendLayout();

            // pnlTop
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 50;

            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Text = "用户管理";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // dgvUsers
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsers.BackgroundColor = System.Drawing.Color.White;
            this.dgvUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvUsers.ColumnHeadersHeight = 30;
            this.dgvUsers.Location = new System.Drawing.Point(12, 60);
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(520, 200);

            // grpAdd
            this.grpAdd.Controls.Add(this.lblNewUsername);
            this.grpAdd.Controls.Add(this.txtNewUsername);
            this.grpAdd.Controls.Add(this.lblNewPassword);
            this.grpAdd.Controls.Add(this.txtNewPassword);
            this.grpAdd.Controls.Add(this.btnAddUser);
            this.grpAdd.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.grpAdd.Location = new System.Drawing.Point(12, 270);
            this.grpAdd.Size = new System.Drawing.Size(520, 60);
            this.grpAdd.Text = "添加用户";

            this.lblNewUsername.AutoSize = true;
            this.lblNewUsername.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblNewUsername.Location = new System.Drawing.Point(15, 25);
            this.lblNewUsername.Text = "账号：";

            this.txtNewUsername.Location = new System.Drawing.Point(62, 22);
            this.txtNewUsername.Size = new System.Drawing.Size(130, 23);

            this.lblNewPassword.AutoSize = true;
            this.lblNewPassword.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblNewPassword.Location = new System.Drawing.Point(210, 25);
            this.lblNewPassword.Text = "密码：";

            this.txtNewPassword.Location = new System.Drawing.Point(257, 22);
            this.txtNewPassword.PasswordChar = '●';
            this.txtNewPassword.Size = new System.Drawing.Size(130, 23);

            this.btnAddUser.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.btnAddUser.FlatAppearance.BorderSize = 0;
            this.btnAddUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddUser.ForeColor = System.Drawing.Color.White;
            this.btnAddUser.Location = new System.Drawing.Point(410, 20);
            this.btnAddUser.Size = new System.Drawing.Size(90, 28);
            this.btnAddUser.Text = "添加";
            this.btnAddUser.UseVisualStyleBackColor = false;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);

            // grpPassword - 修改密码
            this.grpPassword.Controls.Add(this.lblChangePassword);
            this.grpPassword.Controls.Add(this.txtChangePassword);
            this.grpPassword.Controls.Add(this.btnChangePassword);
            this.grpPassword.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.grpPassword.Location = new System.Drawing.Point(12, 340);
            this.grpPassword.Size = new System.Drawing.Size(520, 60);
            this.grpPassword.Text = "修改密码（选中用户后操作）";

            this.lblChangePassword.AutoSize = true;
            this.lblChangePassword.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblChangePassword.Location = new System.Drawing.Point(15, 25);
            this.lblChangePassword.Text = "新密码：";

            this.txtChangePassword.Location = new System.Drawing.Point(75, 22);
            this.txtChangePassword.PasswordChar = '●';
            this.txtChangePassword.Size = new System.Drawing.Size(200, 23);

            this.btnChangePassword.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnChangePassword.FlatAppearance.BorderSize = 0;
            this.btnChangePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangePassword.ForeColor = System.Drawing.Color.White;
            this.btnChangePassword.Location = new System.Drawing.Point(410, 20);
            this.btnChangePassword.Size = new System.Drawing.Size(90, 28);
            this.btnChangePassword.Text = "修改密码";
            this.btnChangePassword.UseVisualStyleBackColor = false;
            this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);

            // btnDeleteUser
            this.btnDeleteUser.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.btnDeleteUser.FlatAppearance.BorderSize = 0;
            this.btnDeleteUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteUser.ForeColor = System.Drawing.Color.White;
            this.btnDeleteUser.Location = new System.Drawing.Point(410, 412);
            this.btnDeleteUser.Size = new System.Drawing.Size(122, 32);
            this.btnDeleteUser.Text = "删除选中用户";
            this.btnDeleteUser.UseVisualStyleBackColor = false;
            this.btnDeleteUser.Click += new System.EventHandler(this.btnDeleteUser_Click);

            // UserManagementForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(545, 460);
            this.Controls.Add(this.btnDeleteUser);
            this.Controls.Add(this.grpPassword);
            this.Controls.Add(this.grpAdd);
            this.Controls.Add(this.dgvUsers);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "用户管理";

            this.pnlTop.ResumeLayout(false);
            this.grpAdd.ResumeLayout(false);
            this.grpAdd.PerformLayout();
            this.grpPassword.ResumeLayout(false);
            this.grpPassword.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.GroupBox grpAdd;
        private System.Windows.Forms.Label lblNewUsername;
        private System.Windows.Forms.TextBox txtNewUsername;
        private System.Windows.Forms.Label lblNewPassword;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.Button btnDeleteUser;
        private System.Windows.Forms.GroupBox grpPassword;
        private System.Windows.Forms.Label lblChangePassword;
        private System.Windows.Forms.TextBox txtChangePassword;
        private System.Windows.Forms.Button btnChangePassword;
    }
}
