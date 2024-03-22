namespace CtrlTabby;

internal static class Program
{
    /// <summary>
    /// Init all the things..
    /// </summary>
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}