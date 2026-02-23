namespace BankingApp.WinForms;

public static class Theme
{
    // Colors from Angular :root
    public static readonly Color Primary      = Color.FromArgb(0x25, 0x63, 0xeb);
    public static readonly Color PrimaryDark  = Color.FromArgb(0x1e, 0x40, 0xaf);
    public static readonly Color Secondary    = Color.FromArgb(0x10, 0xb9, 0x81);
    public static readonly Color Danger       = Color.FromArgb(0xef, 0x44, 0x44);
    public static readonly Color Background  = Color.FromArgb(0xf8, 0xfa, 0xfc);
    public static readonly Color Surface     = Color.White;
    public static readonly Color TextPrimary = Color.FromArgb(0x1e, 0x29, 0x3b);
    public static readonly Color TextSecondary = Color.FromArgb(0x64, 0x74, 0x8b);
    public static readonly Color Border      = Color.FromArgb(0xe2, 0xe8, 0xf0);
    public static readonly Color IconDebitBg = Color.FromArgb(0xfe, 0xe2, 0xe2);
    public static readonly Color IconCreditBg = Color.FromArgb(0xd1, 0xfa, 0xe5);
    public static readonly Color IconTransferBg = Color.FromArgb(0xdb, 0xea, 0xfe);
    public static readonly Color ErrorBg     = Color.FromArgb(0xfe, 0xe2, 0xe2);
    public static readonly Color SuccessBg   = Color.FromArgb(0xd1, 0xfa, 0xe5);

    // Spacing (1rem ≈ 16px)
    public const int PadSmall = 8;
    public const int PadMedium = 16;
    public const int PadLarge = 24;
    public const int PadXLarge = 32;

    // Fonts
    public static Font TitleFont => new Font("Segoe UI", 20, FontStyle.Bold);
    public static Font SubtitleFont => new Font("Segoe UI", 12);
    public static Font SectionFont => new Font("Segoe UI", 14, FontStyle.Bold);
    public static Font BodyFont => new Font("Segoe UI", 9);
    public static Font BodySmallFont => new Font("Segoe UI", 8.25f);
}
