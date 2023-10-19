using System;
using System.Configuration;
using System.Runtime.InteropServices;
using static ResolutionSwitcher.Windows.Definitions;

namespace ResolutionSwitcher
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Determine if we're able to read the system settings okay first before we handle anything!
            _ = GetPrimaryDisplay(out DisplayDevice primaryDisplay);
            if (primaryDisplay.DeviceName == string.Empty)
            {
                throw new InvalidOperationException("Did not find a primary display!");
            }

            _ = GetCurrentDisplaySettings(primaryDisplay, out DEVMODE settings);
            if (settings.dmPelsWidth == 0 || settings.dmPelsHeight == 0 || settings.dmDisplayFrequency == 0)
            {
                throw new InvalidOperationException($"Could not retrieve settings for primary display {primaryDisplay.DeviceName}!");
            }

            // Since I'm the only user, let's assume that the arguments aren't messed up except for basic stuff.
            if (args.Length == 0 || args.Length == 3)
            {
                LoadNewResolution(ref settings, args);

                if (!Supported(ref settings))
                {
                    throw new InvalidProgramException($"Cannot set resolution of {settings.dmPelsWidth}x{settings.dmPelsHeight}@{settings.dmDisplayFrequency}Hz!");
                }

                if(!ChangeResolution(ref settings))
                {
                    throw new InvalidProgramException($"Failed to set resolution of {settings.dmPelsWidth}x{settings.dmPelsHeight}@{settings.dmDisplayFrequency}Hz!");
                }
            }
            else
            {
                throw new ArgumentException("Incorrect number of arguments!");
            }
        }

        /// <summary>
        /// Retrieve the primary display.
        /// </summary>
        /// <returns>Windows API object describing the primary display.</returns>
        private static int GetPrimaryDisplay(out DisplayDevice primaryDisplay)
        {
            primaryDisplay = new DisplayDevice()
            {
                cb = Marshal.SizeOf(typeof(DisplayDevice))
            };

            
            return Functions.EnumDisplayDevices(null, 0, ref primaryDisplay, (int)DisplayDeviceStateFlags.PrimaryDevice);
        }

        /// <summary>
        /// Get the current display settings
        /// </summary>
        /// <param name="display">The display to retrieve settings from</param>
        /// <param name="dm">The DEVMODE object reference</param>
        /// <returns>Return from Win32 function EnumDisplaySettings</returns>
        private static int GetCurrentDisplaySettings(DisplayDevice display, out DEVMODE dm)
        {
            dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));

            return Functions.EnumDisplaySettings(display.DeviceName, ENUM_CURRENT_SETTINGS, ref dm);
        }

        /// <summary>
        /// Determine which resolution to change to and set the <paramref name="settings"/> object to it.
        /// </summary>
        /// <param name="settings">The current display settings</param>
        /// <param name="args">Command line arguments from the user</param>
        private static void LoadNewResolution(ref DEVMODE settings, string[] args)
        {
            int width = 0;
            int height = 0;
            int frequency = 0;

            // Going back to default!
            if (args.Length == 0)
            {
                width = int.Parse(ConfigurationManager.AppSettings["DefaultWidth"]);
                height = int.Parse(ConfigurationManager.AppSettings["DefaultHeight"]);
                frequency = int.Parse(ConfigurationManager.AppSettings["DefaultFrequency"]);
            }
            // Switching to new settings in the format of [width height frequency]
            else
            {
                width = int.Parse(args[0]);
                height = int.Parse(args[1]);
                frequency = int.Parse(args[2]);
            }
            settings.dmPelsWidth = width;
            settings.dmPelsHeight = height;
            settings.dmDisplayFrequency = frequency;
        }

        /// <summary>
        /// Is the resolution supported using the <see cref="CDS_TEST"/> flag to <see cref="Functions.ChangeDisplaySettings(ref DEVMODE, int)"/>
        /// </summary>
        /// <param name="newSettings">The new settings</param>
        /// <returns>True if the resolution change can be completed successfully</returns>
        private static bool Supported(ref DEVMODE newSettings)
        {
            int retVal = Functions.ChangeDisplaySettings(ref newSettings, CDS_TEST);
            return retVal == DISP_CHANGE_SUCCESSFUL;
        }

        /// <summary>
        /// Set the current resolution to the settings provided in <paramref name="newSettings"/>
        /// </summary>
        /// <param name="newSettings">The new resolution settings</param>
        /// <returns>True if the resolution change was completed successfully</returns>
        private static bool ChangeResolution(ref DEVMODE newSettings)
        {
            int retVal = Functions.ChangeDisplaySettings(ref newSettings, CDS_UPDATEREGISTRY);
            return retVal == DISP_CHANGE_SUCCESSFUL;
        }
    }
}
