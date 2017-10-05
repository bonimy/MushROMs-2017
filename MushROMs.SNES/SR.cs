using System.Globalization;
using MushROMs.SNES.Properties;
using HelperSR = Helper.SR;

namespace MushROMs.SNES
{
    internal static class SR
    {
        public static CultureInfo CurrentCulture = CultureInfo.CurrentCulture;

        public static string ErrorRPFSize(string paramName)
        {
            return HelperSR.GetString(Resources.ErrorRPFSize, paramName);
        }

        public static string ErrorTPLHeaderSize(string paramName)
        {
            return HelperSR.GetString(Resources.ErrorTPLHeaderSize, paramName);
        }

        public static string ErrorTPLFormat(string paramName)
        {
            return HelperSR.GetString(Resources.ErrorTPLFormat, paramName);
        }

        public static string ErrorTPLSize(string paramName)
        {
            return HelperSR.GetString(Resources.ErrorTPLSize, paramName);
        }

        public static string ErrorPALSize(string paramName)
        {
            return HelperSR.GetString(Resources.ErrorPALSize, paramName);
        }

        public static string ErrorCHRFormat(string paramName, GraphicsFormat graphicsFormat, int tileSize)
        {
            return HelperSR.GetString(Resources.ErrorCHRFormat, paramName, graphicsFormat, tileSize);
        }
    }
}
