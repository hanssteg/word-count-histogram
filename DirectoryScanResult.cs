using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WordCountAPI
{
    public class DirectoryScanResult
    {
        private readonly List<FileInfo> fileInfos;
        public List<FileInfo> FileInfos
        {
            get { return fileInfos;  }
        }

        private readonly List<string> warnings;
        public List<string> Warnings
        {
            get { return warnings; }
        }

        public DirectoryScanResult()
        {
            fileInfos = new List<FileInfo>();
            warnings = new List<string>();
        }
    }
}