using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;

namespace CoinFill.Helpers
{
    public static class Helper
    {
        public static string GenerateNumbersId()
        {
            return string.Concat(BigInteger.Abs(BigInteger.Parse(Guid.NewGuid().ToString().Replace("-", ""), NumberStyles.AllowHexSpecifier)).ToString().Take(12));
        }

        public static string[] GetObjectIconAndNameAndColor(string iconAndNameAndColorConcatenated)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(iconAndNameAndColorConcatenated))
                {
                    //array[0] -> fa icon
                    //array[1] -> name
                    //array[2] -> color
                    return iconAndNameAndColorConcatenated.Split('|');
                }
                return new string[] { "", "", "" };
            }
            catch (Exception)
            {
                return new string[] { "", "", "" };
            }
        }

        public static string CombinePaths(string path1, string path2)
        {
            if (Path.IsPathRooted(path2))
            {
                path2 = path2.TrimStart(Path.DirectorySeparatorChar);
                path2 = path2.TrimStart(Path.AltDirectorySeparatorChar);
            }

            return Path.Combine(path1, path2);
        }

        public static void EnsureDirectoryExists(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Directory.Exists)
            {
                System.IO.Directory.CreateDirectory(fi.DirectoryName);
            }
        }
    }
}
