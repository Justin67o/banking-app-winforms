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
        Size = new Size(460, 440);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = Theme.Primary;

        var card = new Panel
        {
            BackColor = Theme.Surface,
            BorderStyle = BorderStyle.FixedSingle,
            Size = new Size(380, 340),
            Location = new Point(40, 30)
        };

        var lblTitle = new Label
        {
            Text = "🏦 Banking App",
            Font = Theme.TitleFont,
            ForeColor = Theme.Primary,
            Location = new Point(Theme.PadLarge, 12),
            AutoSize = true
        };
        var lblSub = new Label
        {
            Text = "Sign in to your account",
            ForeColor = Theme.TextSecondary,
            Font = Theme.BodyFont,
            Location = new Point(Theme.PadLarge, 58),
            AutoSize = true
        };
        var lblUser = new Label { Text = "Username", Location = new Point(Theme.PadLarge, 92), ForeColor = Theme.TextPrimary, Font = Theme.BodyFont };
        _txtUsername = new TextBox
        {
            Location = new Point(Theme.PadLarge, 115),
            Size = new Size(320, 24),
            BorderStyle = BorderStyle.FixedSingle,
            Font = Theme.BodyFont
        };
        var lblPass = new Label { Text = "Password", Location = new Point(Theme.PadLarge, 150), ForeColor = Theme.TextPrimary, Font = Theme.BodyFont };
        _txtPassword = new TextBox
        {
            Location = new Point(Theme.PadLarge, 175),
            Size = new Size(320, 24),
            PasswordChar = '*',
            BorderStyle = BorderStyle.FixedSingle,
            Font = Theme.BodyFont
        };
        _lblError = new Label
        {
            Location = new Point(Theme.PadLarge, 208),
            AutoSize = true,
            ForeColor = Theme.Danger,
            BackColor = Theme.ErrorBg,
            Font = Theme.BodySmallFont
        };
        var lblHint = new Label
        {
            Text = "Demo credentials: user / password",
            Location = new Point(Theme.PadLarge, 268),
            AutoSize = true,
            ForeColor = Theme.TextSecondary,
            Font = Theme.BodySmallFont
        };
        var btnSignIn = new Button
        {
            Text = "Sign In",
            Location = new Point(Theme.PadLarge, 233),
            Size = new Size(120, 36),
            BackColor = Theme.Primary,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };
        btnSignIn.Click += BtnSignIn_Click;

        card.Controls.AddRange(new Control[] { lblTitle, lblSub, lblUser, _txtUsername, lblPass, _txtPassword, _lblError, lblHint, btnSignIn });
        Controls.Add(card);
        Load += (_, _) => card.Location = new Point(Math.Max(0, (ClientSize.Width - card.Width) / 2), card.Top);
    
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
            _lblError.Text = "Invalid username or password";
    }
}
