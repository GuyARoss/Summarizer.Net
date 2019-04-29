using System.Collections.Generic;
using System.Linq;

namespace Summarizer.Core
{
    public abstract class ExtractionProvider
    {
        /*
         * Applies the abstract intensity to the amount of keywords by %.
         * Values 0.0 - 1
         */
        public virtual double AbstractIntensity { get; protected set; }

        public ExtractionProvider(double abstractIntensity)
        {
            AbstractIntensity = abstractIntensity;
        }
        public ExtractionProvider()
        {
            AbstractIntensity = .4;
        }

        protected abstract Dictionary<string, double> ExtractKeywords(string statement);
        public virtual Dictionary<string, double> Invoke(string statemnet)
        {
            Dictionary<string, double> frequentKeywords = ExtractKeywords(statemnet);
            var sortedKeywords = SortKeywords(frequentKeywords);

            int size = (int)((AbstractIntensity / 1) * sortedKeywords.Count);

            var sizedKeywords = new Dictionary<string, double>();
            for (int idx = 0; idx < size; idx++)
            {
                var kvp = sortedKeywords.ElementAt(idx);
                sizedKeywords.Add(kvp.Key, kvp.Value);
            }

            return sizedKeywords;
        }
        protected virtual Dictionary<string, double> SortKeywords(Dictionary<string, double> keywords)
        {
            return keywords
                .OrderBy(vec => vec.Value)
                .Reverse()
                .ToDictionary(vec => vec.Key, vec => vec.Value);
        }
    }
}
