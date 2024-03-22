using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace CtrlTabby;

internal static class Program
{
    /// <summary>
    /// Program name and version.
    /// </summary>
    public const string NameAndVersion = "CtrlTabby v0.1-alpha";

    /// <summary>
    /// Init all the things..
    /// </summary>
    [STAThread]
    private static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        if (!ParseCmdArgs(args, out var options, out var error))
        {
            Application.Run(new CmdArgsDialog(error, true));
        }
        else
        {
            Application.Run(new MainForm(options));
        }
    }

    /// <summary>
    /// Parse command line arguments.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    /// <param name="options">Parsed options.</param>
    /// <param name="error">Error message, if any.</param>
    /// <returns>Success.</returns>
    private static bool ParseCmdArgs(
        IReadOnlyList<string> args,
        out Options options,
        [NotNullWhen(returnValue: false)] out string? error)
    {
        options = new();
        error = null;
        
        // -a           Auto-start the process.
        // -i <ms>      Set interval, in milliseconds, between each CTRL+TAB send. Defaults to 3000.
        // -k           Keep going after being interrupted by system events, such as locking the screen.

        var skip = false;

        for (var i = 0; i < args.Count; i++)
        {
            if (skip)
            {
                skip = false;
                continue;
            }

            switch (args[i])
            {
                case "-a":
                    options.AutoStart = true;
                    break;
                
                case "-i":
                    if (i == args.Count - 1)
                    {
                        error = "-i must be followed by a number of milliseconds.";
                        return false;
                    }

                    if (!decimal.TryParse(args[i + 1], NumberStyles.Any, new CultureInfo("en-US"), out var interval) ||
                        interval < 0)
                    {
                        error = "The value for -i must be a positive value.";
                        return false;
                    }

                    options.Interval = interval;
                    skip = true;
                    break;
                
                case "-k":
                    options.StopIfInterrupted = false;
                    break;
                
                default:
                    error = $"Unknown option: {args[i]}";
                    return false;
            }
        }

        return true;
    }
}