using BankingApp.WinForms.Services;
using BankingApp.WinForms;

namespace BankingApp.WinForms;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        if (AuthService.Instance.IsAuthenticated)
        {
            Application.Run(new MainForm());
        }
        else
        {
            var loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
                Application.Run(new MainForm());
        }
    }
}
