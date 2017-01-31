using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WordCountAPI
{
    public class HistogramAPIResponse<T>
    {
        private readonly List<T> histogramData;
        public List<T> HistogramData
        {
            get { return this.histogramData; }
        }

        private readonly List<string> warnings;
        public List<string> Warnings
        {
            get { return warnings; }
        }

        public HistogramAPIResponse(List<T> histogramData, List<string> warnings)
        {
            this.histogramData = histogramData;
            this.warnings = warnings;
        }
    }
}