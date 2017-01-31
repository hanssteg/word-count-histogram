using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WordCountAPI;

namespace WordCountHistogramAPI
{
    public class HistogramBuilder
    {
        private readonly WordCounter wordCounter;
        private DirectoryScanner directoryScanner;

        public HistogramBuilder(WordCounter wordCounter)
        {
            this.wordCounter = wordCounter;
        }

        public Histogram<int> Build(DirectoryScanResult directoryScanResult, List<string> warnings) //good test directory: @"E:\Users\Hans\Desktop\TestDirectory"
        {
            Histogram<int> histogram = new Histogram<int>(warnings);

            List<string> unsearchedFileNames = new List<string>();
            List<string> searchedFileNames = new List<string>();

            foreach (var fileInfo in directoryScanResult.FileInfos)
            {
                int? wordCount = wordCounter.GetWordCount(fileInfo);
                if (wordCount.HasValue)
                {
                    histogram.add(wordCount.Value);
                    searchedFileNames.Add(fileInfo.FullName);
                }
                //else
                //{
                //    //TODO: this should not have .zip...
                //    unsearchedFileNames.Add(fileInfo.FullName);
                //}
            }

            return histogram;
        }
    }
}