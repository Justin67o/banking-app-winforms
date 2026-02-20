using BankingApp.WinForms.Services;

namespace BankingApp.WinForms.Views;

public class TransferView : UserControl
{
    private readonly Action<string, string?> _navigate;
    private ComboBox? _comboFrom;
    private ComboBox? _comboTo;
    private NumericUpDown? _numAmount;
    private TextBox? _txtDesc;
    private Label? _lblError;
    private Label? _lblSuccess;

    public TransferView(Action<string, string?> navigate)
    {
        _navigate = navigate;
        BuildUi();
    }

    private void BuildUi()
    {
        var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, AutoScroll = true, Padding = new Padding(10) };
        flow.Controls.Add(new Label { Text = "Transfer Money", Font = new Font("Segoe UI", 16) });
        flow.Controls.Add(new Label { Text = "Send money between your accounts" });
        flow.Controls.Add(new Label { Text = "From Account" });
        _comboFrom = new ComboBox { Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
        _comboFrom.SelectedIndexChanged += (_, _) =>
        {
            _comboTo!.Items.Clear();
            var fromId = (_comboFrom.SelectedItem as AccountItem)?.Id;
            foreach (var a in AccountService.Instance.GetAccounts().Where(a => a.Id != fromId))
                _comboTo.Items.Add(new AccountItem { Id = a.Id, Text = $"{a.AccountName} ({a.AccountNumber})" });
            _comboTo.DisplayMember = "Text";
        };
        flow.Controls.Add(_comboFrom);
        flow.Controls.Add(new Label { Text = "To Account" });
        _comboTo = new ComboBox { Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
        flow.Controls.Add(_comboTo);
        flow.Controls.Add(new Label { Text = "Amount" });
        _numAmount = new NumericUpDown { Width = 120, Minimum = 0.0m, Maximum = 999999m, DecimalPlaces = 2 };
        flow.Controls.Add(_numAmount);
        flow.Controls.Add(new Label { Text = "Description (optional)" });
        _txtDesc = new TextBox { Width = 250 };
        flow.Controls.Add(_txtDesc);
        _lblError = new Label { AutoSize = true, ForeColor = Color.Red };
        _lblSuccess = new Label { AutoSize = true, ForeColor = Color.Green };
        flow.Controls.Add(_lblError!);
        flow.Controls.Add(_lblSuccess!);
        var btnCancel = new Button { Text = "Cancel", Size = new Size(80, 28) };
        var btnTransfer = new Button { Text = "Transfer Money", Size = new Size(120, 28) };
        btnCancel.Click += (_, _) => ResetForm();
        btnTransfer.Click += btnTransfer_Click;
        flow.Controls.Add(btnCancel);
        flow.Controls.Add(btnTransfer);
        Controls.Add(flow);
    }

    private void btnTransfer_Click(object? sender, EventArgs e)
    {
        _lblError!.Text = "";
        _lblSuccess!.Text = "";
        var from = _comboFrom!.SelectedItem as AccountItem;
        var to = _comboTo!.SelectedItem as AccountItem;
        if (from == null || to == null || _numAmount!.Value <= 0)
        {
            _lblError.Text = "Please fill in all fields correctly.";
            return;
        }
        if (from.Id == to.Id)
        {
            _lblError.Text = "Cannot transfer to the same account.";
            return;
        }
        var acc = AccountService.Instance.GetAccountById(from.Id);
        if (acc == null) return;
        if (_numAmount.Value > acc.Balance)
        {
            _lblError.Text = "Insufficient funds.";
            return;
        }
        TransactionService.Instance.AddTransaction(from.Id, "transfer", _numAmount.Value, string.IsNullOrWhiteSpace(_txtDesc!.Text) ? "Transfer" : _txtDesc.Text.Trim(), to.Id);
        _lblSuccess.Text = $"Successfully transferred {_numAmount.Value:C2}!";
        var timer = new System.Windows.Forms.Timer { Interval = 2000 };
        timer.Tick += (_, _) => { timer.Stop(); _navigate("Transactions", null); };
        timer.Start();
    }

    private void ResetForm()
    {
        _comboFrom!.SelectedIndex = -1;
        _comboTo!.Items.Clear();
        _numAmount!.Value = 0;
        _txtDesc!.Text = "";
        _lblError!.Text = "";
        _lblSuccess!.Text = "";
    }

    public void RefreshData()
    {
        _comboFrom!.Items.Clear();
        foreach (var a in AccountService.Instance.GetAccounts())
            _comboFrom.Items.Add(new AccountItem { Id = a.Id, Text = $"{a.AccountName} ({a.AccountNumber}) - {a.Balance:C2}" });
        _comboFrom.DisplayMember = "Text";
        ResetForm();
    }

    private class AccountItem
    {
        public string Id { get; set; } = "";
        public string Text { get; set; } = "";
    }
}
