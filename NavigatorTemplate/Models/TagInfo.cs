using System;
using System.Collections.Generic;
using System.Text;

namespace NavigatorTemplate.Models
{
    class TagInfo
    {
        public System.IO.DriveInfo driveInfo { get; set; }
        public System.IO.DirectoryInfo directoryInfo { get; set; }
        public Boolean loaded { get; set; }
        public Boolean root { get; set; }
    }
}
