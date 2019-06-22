using Summarizer.Core.KeywordExtractors;

namespace Summarizer.Core.Summarizers
{
    public class TextRankByFrequency : TextRankProvider, ISummarizationLayer
    {
        public TextRankByFrequency(IKeywordExtractor extractor) : base(extractor) { } 

        public TextRankByFrequency()
        {
            KeywordExtractor = new RegularFrequencyExtractor();
        }
    }
}
