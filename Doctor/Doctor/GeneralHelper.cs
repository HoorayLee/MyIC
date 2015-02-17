using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Doctor
{
    class GeneralHelper
    {
        public static string DownloadPicFolder = Environment.CurrentDirectory + "/DownloadFiles/";

        public static Image GetPhoto(string fileName)
        {
            return Image.FromFile(Path.Combine(DownloadPicFolder, fileName));
        }
    }
}
