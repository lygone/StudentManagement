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
            if (MessageBox.Show("确定删除该用户吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                dbService.DeleteUser(userId);
                LoadUsers();
            }
        }
    }
}