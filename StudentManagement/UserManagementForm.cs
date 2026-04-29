using System;
using System.Linq;
using System.Windows.Forms;

namespace StudentManagement
{
    public partial class UserManagementForm : Form
    {
        private DatabaseService dbService = new DatabaseService();

        public UserManagementForm()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            var users = dbService.GetAllUsers();
            dgvUsers.DataSource = null;
            dgvUsers.DataSource = users.Select(u => new
            {
                编号 = u.Id,
                用户名 = u.Username,
                角色 = u.Role,
                创建时间 = u.CreatedAt.ToString("yyyy-MM-dd HH:mm")
            }).ToList();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            string username = txtNewUsername.Text.Trim();
            string password = txtNewPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("账号和密码不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (username.Length < 3)
            {
                MessageBox.Show("用户名长度至少3位！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 4)
            {
                MessageBox.Show("密码长度至少4位！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dbService.AddUser(username, password, "管理员"))
            {
                MessageBox.Show("用户添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers();
                txtNewUsername.Clear();
                txtNewPassword.Clear();
            }
            else
            {
                MessageBox.Show("用户名已存在！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null) return;
            int userId = (int)dgvUsers.CurrentRow.Cells[0].Value;

            var allUsers = dbService.GetAllUsers();
            if (allUsers.Count <= 1)
            {
                MessageBox.Show("不能删除最后一个用户！系统至少需要保留一个管理员。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("确定删除该用户吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                dbService.DeleteUser(userId);
                LoadUsers();
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show("请先在表格中选择要修改密码的用户！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string newPassword = txtChangePassword.Text;
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("请输入新密码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword.Length < 4)
            {
                MessageBox.Show("密码长度至少4位！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = (int)dgvUsers.CurrentRow.Cells[0].Value;
            if (dbService.UpdateUserPassword(userId, newPassword))
            {
                MessageBox.Show("密码修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtChangePassword.Clear();
            }
            else
            {
                MessageBox.Show("修改密码失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
