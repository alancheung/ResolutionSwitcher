using ResolutionSwitcher.Windows;
using System.Runtime.InteropServices;
using static ResolutionSwitcher.Windows.Definitions;

public static class Functions
{
    [DllImport("user32.dll")]
    public static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);

    [DllImport("User32.dll")]
    public static extern int EnumDisplayDevices(string lpDevice, int iDevNum, ref DisplayDevice lpDisplayDevice, int dwFlags);

    [DllImport("User32.dll")]
    public static extern int EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);
}
