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
    private Panel? _contentWrap;
    private Panel? _card;
    private Panel? _wrapper;
    private Panel? _titleWrap;

    public TransferView(Action<string, string?> navigate)
    {
        _navigate = navigate;
        BackColor = Theme.Background;
        BuildUi();
    }

    private void BuildUi()
    {
        var scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Theme.Background };
        _contentWrap = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Background, Padding = new Padding(100, Theme.PadMedium, 100, Theme.PadMedium) };
        var flow = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = true,
            BackColor = Theme.Background,
            Dock = DockStyle.Top
        };

        _titleWrap = new Panel { Height = 70, Margin = new Padding(0, 0, 0, Theme.PadMedium) };
        var titleLbl = new Label { Text = "Transfer Money", Font = Theme.TitleFont, ForeColor = Theme.TextPrimary, AutoSize = true };
        var subtitleLbl = new Label { Text = "Send money between your accounts", Font = Theme.BodyFont, ForeColor = Theme.TextSecondary, AutoSize = true };
        _titleWrap.Controls.Add(titleLbl);
        _titleWrap.Controls.Add(subtitleLbl);
        _titleWrap.Layout += (_, _) =>
        {
            if (_titleWrap == null) return;
            titleLbl.Location = new Point(Math.Max(0, (_titleWrap.ClientSize.Width - titleLbl.Width) / 2), 0);
            subtitleLbl.Location = new Point(Math.Max(0, (_titleWrap.ClientSize.Width - subtitleLbl.Width) / 2), titleLbl.Height + Theme.PadSmall);
        };
        flow.Controls.Add(_titleWrap);

        _wrapper = new Panel { Height = 600, Margin = new Padding(0, 0, 0, Theme.PadMedium) };
        const int cardWidth = 420;
        _card = new Panel { BackColor = Theme.Surface, BorderStyle = BorderStyle.FixedSingle, Size = new Size(cardWidth, 580), Padding = new Padding(Theme.PadXLarge), Location = new Point(0, 0) };
        flow.Controls.Add(_wrapper);
        _wrapper.Controls.Add(_card);

        var y = 24;
        _card.Controls.Add(new Label { Text = "From Account", Location = new Point(20, y), ForeColor = Theme.TextPrimary, Font = Theme.BodyFont }); y += 32;
        _comboFrom = new ComboBox { Location = new Point(20, y), Width = cardWidth - 40, DropDownStyle = ComboBoxStyle.DropDownList, Font = Theme.BodyFont }; y += 44;
        _comboFrom.SelectedIndexChanged += (_, _) =>
        {
            _comboTo!.Items.Clear();
            var fromId = (_comboFrom.SelectedItem as AccountItem)?.Id;
            foreach (var a in AccountService.Instance.GetAccounts().Where(a => a.Id != fromId))
                _comboTo.Items.Add(new AccountItem { Id = a.Id, Text = $"{a.AccountName} ({a.AccountNumber})" });
            _comboTo.DisplayMember = "Text";
        };
        _card.Controls.Add(_comboFrom);

        _card.Controls.Add(new Label { Text = "To Account", Location = new Point(20, y), ForeColor = Theme.TextPrimary, Font = Theme.BodyFont }); y += 32;
        _comboTo = new ComboBox { Location = new Point(20, y), Width = cardWidth - 40, DropDownStyle = ComboBoxStyle.DropDownList, Font = Theme.BodyFont }; y += 44;
        _card.Controls.Add(_comboTo);

        _card.Controls.Add(new Label { Text = "Amount", Location = new Point(20, y), ForeColor = Theme.TextPrimary, Font = Theme.BodyFont }); y += 32;
        _numAmount = new NumericUpDown { Location = new Point(20, y), Width = 140, Minimum = 0.0m, Maximum = 999999m, DecimalPlaces = 2, Font = Theme.BodyFont }; y += 44;
        _card.Controls.Add(_numAmount);

        _card.Controls.Add(new Label { Text = "Description (optional)", Location = new Point(20, y), ForeColor = Theme.TextPrimary, Font = Theme.BodyFont }); y += 32;
        _txtDesc = new TextBox { Location = new Point(20, y), Width = cardWidth - 40, Font = Theme.BodyFont }; y += 44;
        _card.Controls.Add(_txtDesc);

        _lblError = new Label { Location = new Point(20, y), AutoSize = true, ForeColor = Theme.Danger, BackColor = Theme.ErrorBg, Font = Theme.BodySmallFont }; y += 36;
        _lblSuccess = new Label { Location = new Point(20, y), AutoSize = true, ForeColor = Theme.Secondary, BackColor = Theme.SuccessBg, Font = Theme.BodySmallFont }; y += 40;
        _card.Controls.Add(_lblError!);
        _card.Controls.Add(_lblSuccess!);

        var btnCancel = new Button { Text = "Cancel", Location = new Point(20, y), Size = new Size(100, 36), BackColor = Theme.Background, ForeColor = Theme.TextPrimary, FlatStyle = FlatStyle.Flat, Font = Theme.BodyFont };
        var btnTransfer = new Button { Text = "Transfer Money", Location = new Point(130, y), Size = new Size(140, 36), BackColor = Theme.Primary, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = Theme.BodyFont };
        btnCancel.Click += (_, _) => ResetForm();
        btnTransfer.Click += BtnTransfer_Click;
        _card.Controls.Add(btnCancel);
        _card.Controls.Add(btnTransfer);

        _contentWrap.Controls.Add(flow);
        flow.Location = new Point(0, 0);
        scroll.Controls.Add(_contentWrap);
        Controls.Add(scroll);

        void OnContentResize()
        {
            if (_contentWrap == null || _card == null || _wrapper == null) return;
            int clientWidth = _contentWrap.ClientSize.Width;
            int horizontalPad = clientWidth < 900 ? Theme.PadLarge : 100;
            if (_contentWrap.Padding.Left != horizontalPad || _contentWrap.Padding.Right != horizontalPad)
            {
                _contentWrap.Padding = new Padding(horizontalPad, _contentWrap.Padding.Top, horizontalPad, _contentWrap.Padding.Bottom);
            }
            int availableWidth = clientWidth - _contentWrap.Padding.Horizontal;
            if (availableWidth <= 0) return;
            const int cardWidth = 420;
            _card.Width = cardWidth;
            int fieldWidth = cardWidth - 40;
            if (_comboFrom != null) _comboFrom.Width = fieldWidth;
            if (_comboTo != null) _comboTo.Width = fieldWidth;
            if (_txtDesc != null) _txtDesc.Width = fieldWidth;
            _wrapper.Width = availableWidth;
            _card.Left = Math.Max(0, (availableWidth - cardWidth) / 2);
            _card.Top = 0;
            if (_titleWrap != null) _titleWrap.Width = availableWidth;
        }
        _contentWrap.Resize += (_, _) => OnContentResize();
        Load += (_, _) => OnContentResize();
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
