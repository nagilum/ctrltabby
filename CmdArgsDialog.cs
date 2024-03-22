namespace CtrlTabby;

internal sealed class CmdArgsDialog : Form
{
    /// <summary>
    /// Close button.
    /// </summary>
    private Button? _button;
    
    /// <summary>
    /// Command line arguments text.
    /// </summary>
    private Label? _infoLabel;
    
    /// <summary>
    /// Message label, if any.
    /// </summary>
    private Label? _messageLabel;

    /// <summary>
    /// Window width.
    /// </summary>
    private const int WindowWidth = 700;

    /// <summary>
    /// Initialize a new instance of a <see cref="CmdArgsDialog"/> class.
    /// </summary>
    /// <param name="message">Message to display.</param>
    /// <param name="isError">Whether the message is an error or information (default).</param>
    public CmdArgsDialog(string? message = null, bool isError = false)
    {
        this.SetupWindow(message is not null);
        this.AddControls(message, isError);
    }

    /// <summary>
    /// Close button event.
    /// </summary>
    private void CloseButton_Click(object? sender, EventArgs e)
    {
        this.DialogResult = _button!.DialogResult;
        this.Close();
    }

    /// <summary>
    /// Add UI controls and event handling.
    /// </summary>
    /// <param name="message">Message to display.</param>
    /// <param name="isError">Whether the message is an error or information.</param>
    private void AddControls(string? message = null, bool isError = false)
    {
        var top = 9;
        
        if (message is not null)
        {
            _messageLabel = new()
            {
                Location = new(12, top),
                Size = new(WindowWidth - 24, 50),
                Text = message,
                TextAlign = ContentAlignment.MiddleCenter
            };

            if (isError)
            {
                _messageLabel.ForeColor = Color.Crimson;
            }

            this.Controls.Add(_messageLabel);

            top += 59;
        }

        const string infoText
            = """
              -a           Auto-start the process.
              -i <ms>      Set interval, in milliseconds, between each CTRL+TAB send. Defaults to 3000.
              -k           Keep going after being interrupted by system events, such as locking the screen.
              """;

        _infoLabel = new()
        {
            AutoSize = true,
            Font = new Font(FontFamily.GenericMonospace, this.Font.Size),
            Location = new(12, top),
            Size = new(WindowWidth - 24, 50),
            Text = infoText
        };

        const int buttonWidth = WindowWidth / 4;

        _button = new()
        {
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            DialogResult = DialogResult.OK,
            Location = new((WindowWidth - buttonWidth) / 2, top + 50 + _infoLabel.Size.Height),
            Size = new(buttonWidth, 39),
            Text = "&Close"
        };

        _button.Click += this.CloseButton_Click;
        
        this.Controls.Add(_infoLabel);
        this.Controls.Add(_button);
    }

    /// <summary>
    /// Setup dialog window.
    /// </summary>
    /// <param name="hasMessage">Whether it's showing a message or not.</param>
    private void SetupWindow(bool hasMessage)
    {
        this.ClientSize = new(WindowWidth, hasMessage ? 250 : 200);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = $"About {Program.NameAndVersion}";
    }
}