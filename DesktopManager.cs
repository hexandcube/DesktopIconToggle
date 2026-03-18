using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Automation;
using Microsoft.Win32;

namespace DesktopIconToggle;

public class DesktopManager : IDisposable
{
    private const int WH_MOUSE_LL = 14;
    private const int WM_LBUTTONDOWN = 0x0201;

    private NativeMethods.LowLevelMouseProc _proc;
    private IntPtr _hookID = IntPtr.Zero;
    private DateTime _lastClickTime = DateTime.MinValue;
    
    public bool IsPaused { get; set; } = false;

    public DesktopManager()
    {
        _proc = HookCallback;
        _hookID = SetHook(_proc);
    }

    private IntPtr SetHook(NativeMethods.LowLevelMouseProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule!)
        {
            return NativeMethods.SetWindowsHookEx(WH_MOUSE_LL, proc, NativeMethods.GetModuleHandle(curModule.ModuleName!), 0);
        }
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_LBUTTONDOWN && !IsPaused)
        {
            TimeSpan timeSinceLastClick = DateTime.Now - _lastClickTime;
            
            if (timeSinceLastClick.TotalMilliseconds <= SystemInformation.DoubleClickTime)
            {
                _lastClickTime = DateTime.MinValue; 
                
                double x = Cursor.Position.X;
                double y = Cursor.Position.Y;

                Task.Run(() => 
                {
                    if (IsMouseOverDesktopBackground(x, y))
                    {
                        ToggleDesktopIcons();
                    }
                });
            }
            else
            {
                _lastClickTime = DateTime.Now;
            }
        }
        return NativeMethods.CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    private bool IsMouseOverDesktopBackground(double x, double y)
    {
        try
        {
            var pt = new System.Windows.Point(x, y);
            var element = AutomationElement.FromPoint(pt);

            if (element == null) return false;

            if (element.Current.ControlType == ControlType.ListItem)
                return false;

            string className = element.Current.ClassName;
            return className == "SysListView32" || className == "WorkerW" || className == "Progman";
        }
        catch
        {
            return false;
        }
    }

    private void ToggleDesktopIcons()
    {
        IntPtr defView = IntPtr.Zero;
        IntPtr progman = NativeMethods.FindWindow("Progman", null);
        
        defView = NativeMethods.FindWindowEx(progman, IntPtr.Zero, "SHELLDLL_DefView", null);

        if (defView == IntPtr.Zero)
        {
            NativeMethods.EnumWindows((hwnd, lParam) =>
            {
                IntPtr p = NativeMethods.FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null);
                if (p != IntPtr.Zero)
                {
                    defView = p;
                    return false;
                }
                return true;
            }, IntPtr.Zero);
        }

        if (defView != IntPtr.Zero)
        {
            NativeMethods.SendMessage(defView, 0x0111 /*WM_COMMAND*/, (IntPtr)0x7402, IntPtr.Zero);

            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", true);
                if (key != null)
                {
                    int state = (int)(key.GetValue("HideIcons", 1) ?? 1);
                    key.SetValue("HideIcons", state == 1 ? 0 : 1);
                }
            }
            catch {}
        }
    }

    public void Dispose()
    {
        NativeMethods.UnhookWindowsHookEx(_hookID);
    }
}