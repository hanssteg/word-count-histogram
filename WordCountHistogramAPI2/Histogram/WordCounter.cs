using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCountHistogramAPI2.Histogram
{
    public class WordCounter
    {
        private string[] wordSplitters;
        private Func<string, bool> isWord;

        public WordCounter(string[] wordSplitters, Func<string, bool> isWord)
        {
            this.wordSplitters = wordSplitters;
            this.isWord = isWord;
        }

        public Nullable<int> GetWordCount(FileInfo fileInfo)
        {
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException("The file " + fileInfo.FullName + " was not found.");
            }

            switch (fileInfo.Extension)
            {
                case ".txt":
                    return getTxtWordCount(fileInfo);
                default:
                    return null;
            }
        }

        private int getTxtWordCount(FileInfo fileInfo)
        {
            int totalWordCount = 0;

            using (StreamReader streamReader = fileInfo.OpenText())
            {
                string line = streamReader.ReadLine();
                while (line != null)
                {
                    totalWordCount += getTxtWordCount(line);
                    line = streamReader.ReadLine();
                }
            }

            return totalWordCount;
        }

        private int getTxtWordCount(string line)
        {
            if (line == null)
            {
                return 0;
            }

            return line
                .Split(wordSplitters, StringSplitOptions.RemoveEmptyEntries)
                .Where(isWord)
                .Count();
        }
    }
}
