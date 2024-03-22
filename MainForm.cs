using System.ComponentModel;

namespace CtrlTabby;

public sealed class MainForm : Form
{
    /// <summary>
    /// Abort checkbox.
    /// </summary>
    private CheckBox? _abort;
    
    /// <summary>
    /// Start/Stop button.
    /// </summary>
    private Button? _button;

    /// <summary>
    /// Millisecond interval numeric input.
    /// </summary>
    private NumericUpDown? _input;

    /// <summary>
    /// Millisecond info label.
    /// </summary>
    private Label? _label;

    /// <summary>
    /// Send-keys state.
    /// True if we're sending CTRL+TAB keys and user hasn't clicked Stop.
    /// </summary>
    private bool _state;
    
    /// <summary>
    /// Initialize a new instance of a <see cref="MainForm"/> class.
    /// </summary>
    public MainForm()
    {
        this.SetupWindow();
        this.AddControls();
    }

    /// <summary>
    /// Start/Stop button click event.
    /// </summary>
    private void StartStopButton_Click(object? sender, EventArgs e)
    {
        if (_state)
        {
            _state = false;
            return;
        }

        _input!.Enabled = false;
        _abort!.Enabled = false;
        _button!.Text = "&Stop";
        _state = true;
        
        Application.DoEvents();

        const int threshold = 100;

        while (_state)
        {
            try
            {
                while (_state)
                {
                    var ms = 0;

                    while (_state)
                    {
                        ms += threshold;

                        if (ms > _input.Value)
                        {
                            break;
                        }
                    
                        Application.DoEvents();
                        Thread.Sleep(threshold);
                    }
                
                    SendKeys.Send("^{TAB}");
                }
            }
            catch (Exception ex)
            {
                if (_abort.Checked)
                {
                    if (ex is Win32Exception { Message: "Access is denied." })
                    {
                        MessageBox.Show(
                            "Interrupted by system events.",
                            "Stopped",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        MessageBox.Show(
                            ex.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }                
                }
            }

            if (_abort.Checked)
            {
                break;
            }
        }

        _input.Enabled = true;
        _abort.Enabled = true;
        _button.Text = "&Start";
    }

    /// <summary>
    /// Add UI controls and event handling.
    /// </summary>
    private void AddControls()
    {
        _abort = new()
        {
            Checked = true,
            Location = new(12, 57),
            Size = new(207, 25),
            Text = "Stop if interrupted by system events."
        };
        
        _button = new()
        {
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            Location = new(12, 86),
            Size = new(259, 39),
            Text = "&Start"
        };

        _button.Click += this.StartStopButton_Click;

        _input = new()
        {
            Location = new(12, 27),
            Maximum = decimal.MaxValue,
            Minimum = 0,
            Size = new(116, 23),
            Value = 3000
        };

        _label = new()
        {
            AutoSize = true,
            Location = new(12, 9),
            Size = new(207, 15),
            Text = "Milliseconds between each CTRL+TAB"
        };

        this.Controls.Add(_label);
        this.Controls.Add(_input);
        this.Controls.Add(_abort);
        this.Controls.Add(_button);
    }

    /// <summary>
    /// Setup main window.
    /// </summary>
    private void SetupWindow()
    {
        this.ClientSize = new(283, 137);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "CtrlTabby v0.1-alpha";
    }
}