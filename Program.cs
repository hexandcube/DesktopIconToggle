namespace DesktopIconToggle;

static class Program
{
    [STAThread]
    static void Main()
    {
        try
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new TrayApplicationContext());
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Fatal Error during startup:\n\n{ex.Message}\n\n{ex.StackTrace}", 
                            "[DesktopIconToggle] Application Crash", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);
        }
    }
}