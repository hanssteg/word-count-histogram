using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WordCountHistogramAPI
{
    //Note: could extend this to do things like ignore case for strings
    public class Histogram<T>
    {
        private readonly Dictionary<T, int> histogram;
        public Dictionary<T, int> HistogramValue
        {
            get { return histogram; }
        }

        private readonly List<string> warnings;
        public List<string> Warnings
        {
            get { return warnings; }
        }

        public Histogram()
        {
            this.histogram = new Dictionary<T, int>();
            this.warnings = new List<string>();
        }

        public Histogram(List<string> warnings)
        {
            this.histogram = new Dictionary<T, int>();
            this.warnings = warnings;
        }

        public void add(T value)
        {
            if (histogram.ContainsKey(value))
            {
                histogram[value] = histogram[value] + 1;
            }
            else
            {
                histogram.Add(value, 1);
            }
        }
    }
}