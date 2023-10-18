using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static ResolutionSwitcher.Windows.Definitions;

namespace ResolutionSwitcher
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            DisplayDevice primaryDisplay = GetPrimaryDisplay();
            if (primaryDisplay.DeviceName == string.Empty)
            {
                throw new InvalidOperationException("Did not find a primary display!");
            }

            List<(int width, int height, int frequency)> resolutions = GetAllAvailableResolutions(primaryDisplay);
            if (resolutions.Count > 0)
            {
                throw new InvalidOperationException($"No resolutions for primary display {primaryDisplay.DeviceName}!");
            }


        }

        /// <summary>
        /// Retrieve the primary display.
        /// </summary>
        /// <returns>Windows API object describing the primary display.</returns>
        private static DisplayDevice GetPrimaryDisplay()
        {
            DisplayDevice primaryDisplay = new DisplayDevice()
            {
                cb = Marshal.SizeOf(typeof(DisplayDevice))
            };

            Functions.EnumDisplayDevices(null, 0, ref primaryDisplay, (int)DisplayDeviceStateFlags.PrimaryDevice);
            return primaryDisplay;
        }

        /// <summary>
        /// Return all valid screen resolutions for the referenced <paramref name="display"/>.
        /// </summary>
        /// <param name="display">Display to retrieve resolutions for.</param>
        /// <returns>List of all possible display resolutions with the first item being the current resolution.</returns>
        private static List<(int width, int height, int frequency)> GetAllAvailableResolutions(DisplayDevice display)
        {
            List<(int width, int height, int frequency)> resolutions = new List<(int width, int height, int frequency)>();

            DEVMODE dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));

            int retVal = 0;
            int settingNum = ENUM_CURRENT_SETTINGS;
            do
            {
                retVal = Functions.EnumDisplaySettings(display.DeviceName, settingNum++, ref dm);
                if (retVal != 0)
                {
                    resolutions.Add((dm.dmPelsWidth, dm.dmPelsHeight, dm.dmDisplayFrequency));
                }

            } while (retVal != 0);

            return resolutions;
        }
    }
}
