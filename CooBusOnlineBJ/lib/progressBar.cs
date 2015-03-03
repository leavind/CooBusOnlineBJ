using Microsoft.Phone.Shell;

public static class progressBar
{
    static ProgressIndicator prog;
    public static void Show()
    {
        prog = new ProgressIndicator();
        SystemTray.ProgressIndicator = prog;
        prog.IsIndeterminate = true;
        prog.IsVisible = true;
    }

    public static void Show(string title)
    {
        prog = new ProgressIndicator();
        SystemTray.ProgressIndicator = prog;
        prog.Text = "                       " + title;
        prog.IsIndeterminate = true;
        prog.IsVisible = true;
    }

    public static void Hide()
    {
        prog.IsVisible = false;
    }
}
