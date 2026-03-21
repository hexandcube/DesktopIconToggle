namespace DesktopIconToggle;

public class TrayApplicationContext : ApplicationContext
{
    private NotifyIcon _trayIcon;
    private DesktopManager _desktopManager;
    private ToolStripMenuItem _pauseMenuItem;

    private Icon LoadEmbeddedIcon(string fileName, Size? targetSize = null)
    {
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream($"DesktopIconToggle.{fileName}");
        
        if (stream == null)
            throw new System.IO.FileNotFoundException($"Could not find embedded resource: {fileName}");
            
        using Icon baseIcon = new Icon(stream);
        
        if (targetSize.HasValue)
        {
            return new Icon(baseIcon, targetSize.Value);
        }
        
        return new Icon(baseIcon, baseIcon.Size);
    }

    public TrayApplicationContext()
    {
        _desktopManager = new DesktopManager();
        _pauseMenuItem = new ToolStripMenuItem("Pause", null, Pause_Click);

        _trayIcon = new NotifyIcon()
        {
            Icon = LoadEmbeddedIcon("active.ico"),
            ContextMenuStrip = new ContextMenuStrip(),
            Visible = true,
            Text = "DesktopIconToggle"
        };

        _trayIcon.DoubleClick += About_Click;

        _trayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("About", null, About_Click) { Font = new Font("Segoe UI", 9, FontStyle.Bold) });
        _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
        _trayIcon.ContextMenuStrip.Items.Add(_pauseMenuItem);
        _trayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Exit", null, Exit_Click));
    }

    private void About_Click(object? sender, EventArgs e)
    {
        using Form aboutForm = new Form()
        {
            Text = "About DesktopIconToggle",
            Size = new Size(300, 300), 
            StartPosition = FormStartPosition.CenterScreen,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false,
            ShowInTaskbar = false
        };

        PictureBox iconBox = new PictureBox()
        {
            Image = LoadEmbeddedIcon("active.ico", new Size(128, 128)).ToBitmap(),
            SizeMode = PictureBoxSizeMode.Zoom,
            Bounds = new Rectangle(100, 20, 80, 80)
        };

        Label titleLabel = new Label()
        {
            Text = "DesktopIconToggle",
            TextAlign = ContentAlignment.TopCenter,
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            Bounds = new Rectangle(0, 115, 280, 25)
        };

        string url = "https://hexandcube.com";
        string fullText = "Version 2.1.0\n\n" +
                          "Created by Hexandcube\n" +
                          $"{url}\n\n" +
                          "Desktop Icon by Icons8\n";

        LinkLabel infoLabel = new LinkLabel()
        {
            Text = fullText,
            TextAlign = ContentAlignment.TopCenter,
            Font = new Font("Segoe UI", 9),
            Bounds = new Rectangle(0, 145, 280, 150),
            LinkBehavior = LinkBehavior.HoverUnderline,
            ActiveLinkColor = Color.Blue,
            LinkColor = Color.RoyalBlue
        };

        int linkStart = fullText.IndexOf(url);
        infoLabel.LinkArea = new LinkArea(linkStart, url.Length);

        infoLabel.LinkClicked += (s, args) =>
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true });
        };

        aboutForm.Controls.Add(iconBox);
        aboutForm.Controls.Add(titleLabel);
        aboutForm.Controls.Add(infoLabel);

        aboutForm.ShowDialog();
    }

    private void Pause_Click(object? sender, EventArgs e)
    {
        _desktopManager.IsPaused = !_desktopManager.IsPaused;
        
        if (_desktopManager.IsPaused)
        {
            _pauseMenuItem.Text = "Unpause";
            _trayIcon.Icon = LoadEmbeddedIcon("suspended.ico");
        }
        else
        {
            _pauseMenuItem.Text = "Pause";
            _trayIcon.Icon = LoadEmbeddedIcon("active.ico");
        }
    }

    private void Exit_Click(object? sender, EventArgs e)
    {
        _trayIcon.Visible = false;
        _desktopManager.Dispose();
        Application.Exit();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _trayIcon?.Dispose();
            _desktopManager?.Dispose();
            _pauseMenuItem?.Dispose();
        }
        base.Dispose(disposing);
    }
}