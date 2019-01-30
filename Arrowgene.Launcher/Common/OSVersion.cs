using System;

namespace Arrowgene.Launcher.Common
{
    public class OSVersionInfo
    {

        public static OSType GetOsType()
        {
            OperatingSystem osInfo = Environment.OSVersion;
            switch (osInfo.Platform)
            {
                case PlatformID.Win32Windows:
                    switch (osInfo.Version.Minor)
                    {
                        case 0: return OSType.Windows_95;
                        case 10:
                            switch (osInfo.Version.Revision.ToString())
                            {
                                case "2222A": return OSType.Windows_98_Second_Edition;
                                default: return OSType.Windows_98;
                            }
                        case 90: return OSType.Windows_Me;
                        default: return OSType.Unknown;
                    }
                case PlatformID.Win32NT:
                    switch (osInfo.Version.Major)
                    {
                        case 3: return OSType.Windows_NT_3_51;
                        case 4: return OSType.Windows_NT_4_0;
                        case 5:
                            switch (osInfo.Version.Minor)
                            {
                                case 0: return OSType.Windows_2000;
                                case 1: return OSType.Windows_XP;
                                case 2: return OSType.Windows_2003;
                                default: return OSType.Windows_2000;
                            }
                        case 6:
                            switch (osInfo.Version.Minor)
                            {
                                case 0: return OSType.Windows_Vista;
                                case 1: return OSType.Windows_7;
                                case 2: return OSType.Windows_8;
                                case 3: return OSType.Windows_8_1;
                                default: return OSType.Windows_Vista;
                            }
                        case 10: return OSType.Windows_10;
                        default: return OSType.Unknown;
                    }
                default: return OSType.Unknown;
            }
        }
    }
}
