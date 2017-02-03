using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WordCountHistogramAPI2.Histogram;

namespace WordCountHistogramAPI2
{
    public class WordCountListController : ApiController
    {
        //Note: could read in entire file and allow \n to not be a separator, but would have to read entire file into
        //memory or write some more complicated logic to merge two lines together and then drop the first and read
        //another onto the end of the remaining file...
        private readonly string[] wordSpepatators = new string[] { " ", "\t" };

        private readonly HistogramBuilder histogramBuilder;
        private readonly WordCounter wordCounter;

        public WordCountListController()
        {
            this.wordCounter = new WordCounter(wordSpepatators, isWord);
            this.histogramBuilder = new HistogramBuilder(this.wordCounter);
        }

        public HttpResponseMessage Get(String directory)
        {
            if (String.IsNullOrWhiteSpace(directory))
            {
                return BuildError("A directory must be entered.");
            }
            if (directory.Length < 4 || !(directory.Substring(1, 2).Equals(":/") || directory.Substring(1, 2).Equals(":\\")))
            {
                return BuildError("Invlid Directory: [" + directory + "], Must be an absolut path.");
            }
            if (!Directory.Exists(directory))
            {
                return BuildError("Directory not found: [" + directory + "]");
            }

            try
            {
                DirectoryInfo directoryToExtractTo = new DirectoryInfo(new Guid().ToString() + "zipDir");

                DirectoryScanner directoryScanner = new DirectoryScanner(directoryToExtractTo, new List<String> { ".zip" });

                DirectoryInfo directoryInfo = new DirectoryInfo(directory);

                DirectoryScanResult result = directoryScanner.Scan(directoryInfo);

                Histogram<int> histogram = histogramBuilder.Build(result, result.Warnings);

                List<int> wordCounts = new List<int>();
                foreach (var pair in histogram.HistogramValue.ToList())
                {
                    int wordCount = pair.Key;
                    int numFiles = pair.Value;

                    for (int i = 0; i < numFiles; i++)
                    {
                        wordCounts.Add(wordCount);
                    }
                }

                HistogramAPIResponse<int> response = new HistogramAPIResponse<int>(wordCounts, histogram.Warnings);

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        private HttpResponseMessage BuildError(String message)
        {
            HttpError error = new HttpError(message);
            return Request.CreateResponse(HttpStatusCode.BadRequest, error);
        }

        //Note: right now word is defined as not whitespace and not containing digits, think if this could be better...
        private static bool isWord(string input)
        {
            return !String.IsNullOrWhiteSpace(input) && !input.Any(char.IsDigit);
        }
    } 
}