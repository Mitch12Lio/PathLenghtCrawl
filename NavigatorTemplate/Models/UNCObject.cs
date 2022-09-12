using System;
using System.Collections.Generic;
using System.Text;

namespace NavigatorTemplate.Models
{
    public class UNCObject
    {
        public int CharacterCount { get; set; }
        public string NameUNC { get; set; }
        public bool Folder { get; set; }
        public string NameObject { get; set; }
        public string NameNetwork { get; set; }

        public List<System.IO.FileInfo> fileInfos = new List<System.IO.FileInfo>();
    }
}
