<img src="https://raw.githubusercontent.com/hexandcube/DesktopIconToggle/refs/heads/main/active.ico" width="64" height="64" alt="DesktopIconToggle Icon">

# DesktopIconToggle

Desktop Icon Toggle is a lightweight system tray utility that allows you to instantly toggle Windows desktop icons with a double-click.

## Installation and Usage

### Using the installer

[⬇️ Download DesktopIconToggle-Setup.exe](https://github.com/hexandcube/DesktopIconToggle/releases/latest/download/DesktopIconToggle-Setup.exe)

Simply download and launch the installer. The app should launch automatically after installation, and will autostart on boot.
You can also launch it manually from the Start Menu.

### Using a package manager (community packages)

Install using Chocolatey: `choco install desktopicontoggle`

Install using WinGet: `winget install -e --id Hexandcube.DesktopIconToggle`

The app won't launch automatically after a silent install, you need to launch it manually, or restart Windows for it to launch on startup.

### Standalone executable

[⬇️ Download DesktopIconToggle-x64.zip (for x64)](https://github.com/hexandcube/DesktopIconToggle/releases/latest/download/DesktopIconToggle-x64.zip)

[⬇️ Download DesktopIconToggle-arm64.zip (for ARM64)](https://github.com/hexandcube/DesktopIconToggle/releases/latest/download/DesktopIconToggle-arm64.zip)

In the zip file, 2 different executables are provided:

- **STANDARD** (DesktopIconToggle-win_x64.exe / DesktopIconToggle-win_arm64.exe)

  Lightweight executable, requires [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) to be installed on your system 

- **PORTABLE** (DesktopIconToggle-win_x64-Portable.exe / DesktopIconToggle-win_arm64-Portable.exe)

  A heavier, self-contained executable, includes all necessary dependencies.

## Attributions

Desktop icon by [Icons8](https://icons8.com/)

Special thanks to [all contributors](https://github.com/hexandcube/DesktopIconToggle/graphs/contributors)

## License

DesktopIconToggle by Hexandcube is licensed under the [MIT License](https://github.com/hexandcube/DesktopIconToggle/blob/main/LICENSE.md)