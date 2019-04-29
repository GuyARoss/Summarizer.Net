using System.Linq;
using System.Collections.Generic;

namespace Summarizer.Core.KeywordExtractors
{
    /*
     *  Vectorizes keywords based on how frequently they appear within the statement's context.
     */
    public class RegularFrequencyExtractor : ExtractionProvider, IKeywordExtractor
    {
        public RegularFrequencyExtractor() : base() { }
        public RegularFrequencyExtractor(double abstractIntensity) : base(abstractIntensity) { }

        public Dictionary<string, double> Invoke(Dictionary<string, double> scoredStatements)
        {
            var words = new List<string>();
            foreach (var scoredStatement in scoredStatements)
            {
                string[] statementWords = scoredStatement.Key
                    .Split(' ');

                foreach(string statementWord in statementWords)
                {
                    words.Add(statementWord);
                }
            }

            return _scoreKeywords(words, scoredStatements);
        }

        protected override Dictionary<string, double> ExtractKeywords(string statement)
        {
            var words = statement
                .Split(' ')
                .ToList();

            return _scoreKeywords(words, new Dictionary<string, double>());
        }

        private Dictionary<string, double> _scoreKeywords(List<string> words, Dictionary<string, double> scoredStatements)
        {
            foreach (string word in words)
            {
                if (scoredStatements.ContainsKey(word))
                {
                    scoredStatements[word] = scoredStatements[word] + 1.0;
                }
                else
                {
                    scoredStatements.Add(word, 0.01);
                }
            }

            return scoredStatements;
        }
    }
}
