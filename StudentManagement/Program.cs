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

            var dbService = new DatabaseService();
            if (!dbService.HasAnyUser())
            {
                var initForm = new AdminInitForm(dbService);
                if (initForm.ShowDialog() != DialogResult.OK)
                    return;
            }

            LoginForm login = new LoginForm();
            if (login.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new Form1());
            }

            DatabaseService.Shutdown();
        }
    }
}
