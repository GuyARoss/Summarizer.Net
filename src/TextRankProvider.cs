using System.Linq;
using System.Collections.Generic;

namespace Summarizer.Core
{
    public abstract class TextRankProvider
    {
        public virtual IKeywordExtractor KeywordExtractor { get; protected set; }

        public TextRankProvider() { }
        public TextRankProvider(IKeywordExtractor extractor)
        {
            KeywordExtractor = extractor;
        }

        public virtual Dictionary<string, double> Invoke(string statement)
        {
            Dictionary<string, double> scoredKeywords = GenerateScoredKeywords(statement);

            List<string> sentences = ConvertStatementToSentences(statement);
            Dictionary<string, double> assignedVectors = AssignUniqueVectorsFromEmbededKeywords(sentences, scoredKeywords);

            return assignedVectors;
        }
        public virtual Dictionary<string, double> Invoke(Dictionary<string, double> scoredStatements)
        {
            Dictionary<string, double> scoredKeywords = GenerateScoredKeywords(scoredStatements);

            var sentences = scoredStatements
                .Select(x => x.Key)
                .ToList();

            return AssignUniqueVectorsFromEmbededKeywords(sentences, scoredStatements);
        }

        protected virtual Dictionary<string, double> AssignUniqueVectorsFromEmbededKeywords(List<string> sentences, Dictionary<string, double> scoredKeywords)
        {
            var sentenceScoreVector = new Dictionary<string, double>();

            foreach (string sentence in sentences)
            {
                double foundKeywordsInSentence = 0.0;

                string[] words = sentence.Split(' ');
                foreach (string word in words)
                {
                    if (scoredKeywords.ContainsKey(word))
                    {
                        foundKeywordsInSentence += scoredKeywords[word];
                    }
                }

                sentenceScoreVector[sentence] = foundKeywordsInSentence;
            }

            return sentenceScoreVector;
        }

        protected virtual Dictionary<string, double> GenerateScoredKeywords(string statement)
        {
            return KeywordExtractor.Invoke(statement);
        }
        protected virtual Dictionary<string, double> GenerateScoredKeywords(Dictionary<string, double> scoredStatements)
        {
            return KeywordExtractor.Invoke(scoredStatements);
        }

        protected virtual List<string> ConvertStatementToSentences(string statement)
        {
            return SummarizationHelper.ConvertStatementToSentences(statement);
        }
    }
}
