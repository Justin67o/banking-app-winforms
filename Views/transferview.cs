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
        BackColor = Theme.Background;
        BuildUi();
    }

        private void BuildUi()
    {
        var scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Theme.Background };
        var contentWrap = new Panel { AutoSize = true, BackColor = Theme.Background, MaximumSize = new Size(800, 0) };
        var flow = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = true,
            BackColor = Theme.Background,
            Padding = new Padding(Theme.PadMedium)
        };

        flow.Controls.Add(new Label { Text = "Transfer Money", Font = Theme.TitleFont, ForeColor = Theme.TextPrimary, AutoSize = true, Margin = new Padding(-Theme.PadMedium, 0, 0, 0) });
        flow.Controls.Add(new Label { Text = "Send money between your accounts", Font = Theme.BodyFont, ForeColor = Theme.TextSecondary, AutoSize = true, Margin = new Padding(0, Theme.PadSmall, 0, Theme.PadLarge) });

        var card = new Panel { BackColor = Theme.Surface, BorderStyle = BorderStyle.FixedSingle, Size = new Size(520, 380), Padding = new Padding(Theme.PadXLarge) };
        flow.Controls.Add(card);

        var y = 20;
        card.Controls.Add(new Label { Text = "From Account", Location = new Point(20, y), ForeColor = Theme.TextPrimary, Font = Theme.BodyFont }); y += 24;
        _comboFrom = new ComboBox { Location = new Point(20, y), Width = 400, DropDownStyle = ComboBoxStyle.DropDownList, Font = Theme.BodyFont }; y += 32;
        _comboFrom.SelectedIndexChanged += (_, _) =>
        {
            _comboTo!.Items.Clear();
            var fromId = (_comboFrom.SelectedItem as AccountItem)?.Id;
            foreach (var a in AccountService.Instance.GetAccounts().Where(a => a.Id != fromId))
                _comboTo.Items.Add(new AccountItem { Id = a.Id, Text = $"{a.AccountName} ({a.AccountNumber})" });
            _comboTo.DisplayMember = "Text";
        };
        card.Controls.Add(_comboFrom);

        card.Controls.Add(new Label { Text = "To Account", Location = new Point(20, y), ForeColor = Theme.TextPrimary, Font = Theme.BodyFont }); y += 24;
        _comboTo = new ComboBox { Location = new Point(20, y), Width = 400, DropDownStyle = ComboBoxStyle.DropDownList, Font = Theme.BodyFont }; y += 32;
        card.Controls.Add(_comboTo);

        card.Controls.Add(new Label { Text = "Amount", Location = new Point(20, y), ForeColor = Theme.TextPrimary, Font = Theme.BodyFont }); y += 24;
        _numAmount = new NumericUpDown { Location = new Point(20, y), Width = 140, Minimum = 0.0m, Maximum = 999999m, DecimalPlaces = 2, Font = Theme.BodyFont }; y += 36;
        card.Controls.Add(_numAmount);

        card.Controls.Add(new Label { Text = "Description (optional)", Location = new Point(20, y), ForeColor = Theme.TextPrimary, Font = Theme.BodyFont }); y += 24;
        _txtDesc = new TextBox { Location = new Point(20, y), Width = 400, Font = Theme.BodyFont }; y += 36;
        card.Controls.Add(_txtDesc);

        _lblError = new Label { Location = new Point(20, y), AutoSize = true, ForeColor = Theme.Danger, BackColor = Theme.ErrorBg, Font = Theme.BodySmallFont }; y += 28;
        _lblSuccess = new Label { Location = new Point(20, y), AutoSize = true, ForeColor = Theme.Secondary, BackColor = Theme.SuccessBg, Font = Theme.BodySmallFont }; y += 32;
        card.Controls.Add(_lblError!);
        card.Controls.Add(_lblSuccess!);

        var btnCancel = new Button { Text = "Cancel", Location = new Point(20, y), Size = new Size(100, 36), BackColor = Theme.Background, ForeColor = Theme.TextPrimary, FlatStyle = FlatStyle.Flat, Font = Theme.BodyFont };
        var btnTransfer = new Button { Text = "Transfer Money", Location = new Point(130, y), Size = new Size(140, 36), BackColor = Theme.Primary, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = Theme.BodyFont };
        btnCancel.Click += (_, _) => ResetForm();
        btnTransfer.Click += BtnTransfer_Click;
        card.Controls.Add(btnCancel);
        card.Controls.Add(btnTransfer);

        contentWrap.Controls.Add(flow);
        flow.Location = new Point(0, Theme.PadMedium);
        scroll.Controls.Add(contentWrap);
        contentWrap.Location = new Point(0, 0);
        Controls.Add(scroll);
    }

    private void BtnTransfer_Click(object? sender, EventArgs e)
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
