using BankingApp.WinForms.Services;

namespace BankingApp.WinForms;

public class LoginForm : Form
{
    private readonly TextBox _txtUsername;
    private readonly TextBox _txtPassword;
    private readonly Label _lblError;

    public LoginForm()
    {
        Text = "Banking App";
        Size = new Size(300, 320);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        StartPosition = FormStartPosition.CenterScreen;

        var lblTitle = new Label
        {
            Text = "🏦 Banking App",
            Font = new Font("Segoe UI", 14),
            Location = new Point(20, 20),
            AutoSize = true
        };
        var lblSub = new Label
        {
            Text = "Sign in to your account",
            Location = new Point(20, 50),
            AutoSize = true
        };
        var lblUser = new Label { Text = "Username", Location = new Point(20, 85) };
        _txtUsername = new TextBox { Location = new Point(20, 105), Size = new Size(240, 23) };
        var lblPass = new Label { Text = "Password", Location = new Point(20, 135) };
        _txtPassword = new TextBox { Location = new Point(20, 155), Size = new Size(240, 23), PasswordChar = '*' };
        _lblError = new Label { Location = new Point(20, 185), AutoSize = true, ForeColor = Color.Red };
        var lblHint = new Label { Text = "Demo credentials: user / password", Location = new Point(20, 250), AutoSize = true };
        var btnSignIn = new Button { Text = "Sign In", Location = new Point(20, 210), Size = new Size(100, 30) };

        btnSignIn.Click += BtnSignIn_Click;

        Controls.Add(lblTitle);
        Controls.Add(lblSub);
        Controls.Add(lblUser);
        Controls.Add(_txtUsername);
        Controls.Add(lblPass);
        Controls.Add(_txtPassword);
        Controls.Add(_lblError);
        Controls.Add(lblHint);
        Controls.Add(btnSignIn);
    }

    private void BtnSignIn_Click(object? sender, EventArgs e)
    {
        _lblError.Text = "";
        if (AuthService.Instance.Login(_txtUsername.Text.Trim(), _txtPassword.Text))
        {
            DialogResult = DialogResult.OK;
            Close();
        }
        else
        {
            _lblError.Text = "Invalid username or password";
        }
    }
}
