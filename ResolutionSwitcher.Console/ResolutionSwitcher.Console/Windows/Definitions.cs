using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ResolutionSwitcher.Windows
{
    public class Definitions
    {
        public const int ENUM_CURRENT_SETTINGS = -1;

        /// <summary>
        /// Flag argument for <see cref="Functions.ChangeDisplaySettings(ref DEVMODE, int)"/>
        /// </summary>
        public const int CDS_UPDATEREGISTRY = 0x01;
        /// <summary>
        /// Flag argument for <see cref="Functions.ChangeDisplaySettings(ref DEVMODE, int)"/>
        /// </summary>
        public const int CDS_TEST = 0x02;

        /// <summary>
        /// Possible return value from <see cref="Functions.ChangeDisplaySettings(ref DEVMODE, int)"/>
        /// </summary>
        public const int DISP_CHANGE_SUCCESSFUL = 0;
        /// <summary>
        /// Possible return value from <see cref="Functions.ChangeDisplaySettings(ref DEVMODE, int)"/>
        /// </summary>
        public const int DISP_CHANGE_RESTART = 1;
        /// <summary>
        /// Possible return value from <see cref="Functions.ChangeDisplaySettings(ref DEVMODE, int)"/>
        /// </summary>
        public const int DISP_CHANGE_FAILED = -1;

        [Flags]
        public enum DisplayDeviceStateFlags : int
        {
            /// <summary>The device is part of the desktop.</summary>
            AttachedToDesktop = 0x1,
            MultiDriver = 0x2,
            /// <summary>The device is part of the desktop.</summary>
            PrimaryDevice = 0x4,
            /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
            MirroringDriver = 0x8,
            /// <summary>The device is VGA compatible.</summary>
            VGACompatible = 0x16,
            /// <summary>The device is removable; it cannot be the primary display.</summary>
            Removable = 0x20,
            /// <summary>The device has more display modes than its output devices support.</summary>
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            private const int CCHDEVICENAME = 32;
            private const int CCHFORMNAME = 32;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DisplayDevice
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            [MarshalAs(UnmanagedType.U4)]
            public DisplayDeviceStateFlags StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }
    }
}
