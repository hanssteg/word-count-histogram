using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using WordCountAPI;

namespace WordCountHistogramAPI
{
    public class DirectoryScanner : IDisposable
    {
        private const string directoryTooLongWarning = "Some directories were skipped because there pathes are too long.";

        private readonly DirectoryInfo directoryToExtractTo;
        private readonly List<String> zipExtentions;

        public DirectoryScanner(DirectoryInfo zipDirectory, List<String> zipExtentions)
        {
            this.directoryToExtractTo = zipDirectory;
            this.zipExtentions = zipExtentions;
        }

        public DirectoryScanResult Scan(DirectoryInfo directoryInfo)
        {
            DirectoryScanResult result = new DirectoryScanResult();
            Scan(directoryInfo, result);
            return result;
        }

        private void Scan(DirectoryInfo directory, DirectoryScanResult directoryScanResult)
        {
            try
            {
                foreach (FileInfo fileInfo in directory.EnumerateFiles())
                {
                    if (zipExtentions.Contains(fileInfo.Extension))
                    {
                        unzipAndScan(fileInfo, directoryScanResult);
                    }
                    else //Do not add unzipped files
                    {
                        directoryScanResult.FileInfos.Add(fileInfo);
                    }
                }
                foreach (DirectoryInfo directoryInfo in directory.EnumerateDirectories())
                {
                    Scan(directoryInfo, directoryScanResult);
                }
            }
            catch (PathTooLongException e)
            {
                if(!directoryScanResult.Warnings.Any(w => w.Equals(directoryTooLongWarning)))
                {
                    directoryScanResult.Warnings.Add(directoryTooLongWarning);
                }
            }
        }

        private void unzipAndScan(FileInfo fileInfo, DirectoryScanResult directoryScanResult)
        {
            //Note: could use a DirectoryInfoBuilder and inject with the constructor to make more testable
            DirectoryInfo unzipHere = new DirectoryInfo(Path.GetTempPath() + "\\" + Guid.NewGuid().ToString());

            unzipHere.Create();

            //Note: SHould wrap this is it's own class to make more testable since it is static
            //Note: throws System.IO.InvalidDataException if not a valid unzippable file
            ZipFile.ExtractToDirectory(fileInfo.FullName, unzipHere.FullName);

            Scan(unzipHere, directoryScanResult);
        }

        public void Dispose()
        {
            if (this.directoryToExtractTo.Exists) { this.directoryToExtractTo.Delete(); }
        }
    }
}