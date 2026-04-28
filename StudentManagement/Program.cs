using System;
using System.Windows.Forms;

namespace StudentManagement
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 检查是否有用户，若无则弹出初始化窗口
            var dbService = new DatabaseService();
            if (!dbService.HasAnyUser())
            {
                // 首次使用，创建默认管理员
                var initForm = new AdminInitForm(dbService);
                if (initForm.ShowDialog() != DialogResult.OK)
                    return; // 用户取消初始化
            }

            LoginForm login = new LoginForm();
            if (login.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new Form1());
            }
        }
    }
}