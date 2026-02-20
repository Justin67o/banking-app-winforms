namespace BankingApp.WinForms.Services;

public class AuthService
{
    public static AuthService Instance { get; } = new AuthService();
    private const string DefaultUsername = "user";
    private const string DefaultPassword = "password";

    private bool _isAuthenticated;
    private string _username = "";

    public bool Login(string username, string password)
    {
        if (username == DefaultUsername && password == DefaultPassword)
        {
            _isAuthenticated = true;
            _username = username;
            return true;
        }
        return false;
    }

    public void Logout()
    {
        _isAuthenticated = false;
        _username = "";
    }

    public bool IsAuthenticated => _isAuthenticated;
    public string GetUsername() => _username;
}
