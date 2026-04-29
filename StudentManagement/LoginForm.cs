using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManagement
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            AcceptButton = btnLogin;

            var origColor = btnLogin.BackColor;
            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = Brighten(origColor, 1.15f);
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = origColor;
            btnLogin.MouseDown += (s, e) => btnLogin.BackColor = Darken(origColor, 0.85f);
            btnLogin.MouseUp += (s, e) => btnLogin.BackColor = btnLogin.ClientRectangle.Contains(btnLogin.PointToClient(Cursor.Position)) ? Brighten(origColor, 1.15f) : origColor;
        }

        private static Color Brighten(Color c, float f) => Color.FromArgb(c.A, Math.Min(255, (int)(c.R * f)), Math.Min(255, (int)(c.G * f)), Math.Min(255, (int)(c.B * f)));
        private static Color Darken(Color c, float f) => Color.FromArgb(c.A, (int)(c.R * f), (int)(c.G * f), (int)(c.B * f));

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var origColor = btnLogin.BackColor;
            btnLogin.BackColor = Darken(origColor, 0.85f);
            btnLogin.Refresh();
            await Task.Delay(100);
            btnLogin.BackColor = origColor;

            string username = txtAccount.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("请输入账号和密码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dbService = new DatabaseService();
            var user = dbService.AuthenticateUser(username, password);
            if (user != null)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("账号或密码错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
    }
}
