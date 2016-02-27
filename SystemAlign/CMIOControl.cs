using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infragistics.UltraChart.Shared.Styles;

namespace SystemAlign
{
    class CWKBIOControl
    {
        public long GetDirectorySize(string path)
        {
            long dirSize = 0;
            DirectoryInfo dir = new DirectoryInfo(path);

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                dirSize += file.Length;
            }

            foreach (DirectoryInfo dr in dir.GetDirectories())
            {
                dirSize += GetDirectorySize(dr.FullName);
            }

            return dirSize/1024/2014;
        }

        private string DiskSize = string.Empty;
        private string UsedSize = string.Empty;

        public string GetSet_DiskSize
        {
            get { return DiskSize; }
            set { DiskSize = value; }
        }

        public string GetSet_UsedSize
        {
            get { return UsedSize; }
            set { UsedSize = value; }
        }

        public string GetDiskSpace(string drive)
        {
            System.IO.DriveInfo drvInfo = new DriveInfo(drive);

            string ttlSize = (drvInfo.TotalSize/1024f/1024f/1024f).ToString("0.00");
            string usedSize = ((drvInfo.TotalSize - drvInfo.TotalFreeSpace)/1024f/1024f/1024f).ToString("0.00");

            DiskSize = ttlSize;
            UsedSize = usedSize;

            return usedSize + " GB  /  " + ttlSize+" GB";
        }
    }
}
