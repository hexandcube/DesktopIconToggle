namespace DesktopIconToggle;


static class Program
{
    private const string MutexName = "Global\\DesktopIconToggle_370A329E-902E-4B19-A164-34D43A1F5014";

    [STAThread]
    static void Main()
    {

        using (System.Threading.Mutex mutex = new System.Threading.Mutex(false, MutexName, out bool createdNew)) {
            if (!createdNew)
            {
                MessageBox.Show("An instance of DesktopIconToggle is already running in the background. To pause or exit, right-click it's icon in the system tray.",
                                "DesktopIconToggle is already running",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }
            
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
}