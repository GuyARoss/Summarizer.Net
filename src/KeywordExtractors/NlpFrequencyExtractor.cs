using System.Linq;
using System.Collections.Generic;

using Summarizer.Infrastructure.Nlp;

namespace Summarizer.Core.KeywordExtractors
{
    public class NlpFrequencyExtractor : ExtractionProvider, IKeywordExtractor
    {
        public readonly IEnumerable<PosType> OmittedTokens;

        public NlpFrequencyExtractor() : base()
        {
            _setupOmmitedTokens(ref OmittedTokens);
        }
        public NlpFrequencyExtractor(double abstractIntensity) : base(abstractIntensity)
        {
            _setupOmmitedTokens(ref OmittedTokens);
        }       
        public NlpFrequencyExtractor(double abstractIntensity, IEnumerable<PosType> omittedPartsOfSpeech) : base(abstractIntensity)
        {
            OmittedTokens = omittedPartsOfSpeech;
        }
        public NlpFrequencyExtractor(IEnumerable<PosType> omittedPartsOfSpeech)
        {
            OmittedTokens = omittedPartsOfSpeech;
        }


        public Dictionary<string, double> Invoke(Dictionary<string, double> scoredStatements)
        {
            string statement = string.Join(",", scoredStatements.Select(x => x.Key).Select(x => x.ToString()).ToArray());

            return NLPExtractor.KeywordsByFrequency(statement, OmittedTokens, 50, scoredStatements);
        }
        protected override Dictionary<string, double> ExtractKeywords(string statement)
        {
            return NLPExtractor.KeywordsByFrequency(statement, OmittedTokens, 50);
        }
        private static void _setupOmmitedTokens(ref IEnumerable<PosType> omittedTokensField)
        {
            omittedTokensField = new List<PosType> {
                PosType.WTF_TWO,
                PosType.VBP,
                PosType.WTF,
                PosType.DT,
                PosType.RB,
                PosType.IN,
                PosType.VBZ,
                PosType.TO,
                PosType.CC,
                PosType.VB,
                PosType.PRP,
                PosType.MD,
                PosType.VBD,
                PosType.PRPS
            };
        }
    }
}
