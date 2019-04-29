using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Summarizer.Infrastructure.Nlp
{
    public static class NLPExtractor
    {
        // Note: this method could use improvement.
        public static Dictionary<string, double> KeywordsByFrequency(string statement, IEnumerable<PosType> badTokens, int maxRange, Dictionary<string, double> scoredKeywords = null)
        {
            string cleanedStatement = Regex.Replace(statement, @"(\;+)", "").Replace("(", "").Replace(")", "");
            cleanedStatement = Regex.Replace(statement, "[^0-9A-Za-z ,]", "");

            string[] tokens = cleanedStatement.TokenizeStatement();
            string[] taggedTokens = tokens.TagPartsOfSpeech();

            var unManagedKeywords = new List<string>();
            for(int idx = 0; idx < tokens.Length; idx++)
            {               
                if (!badTokens.Any(token => token.ToStringPos() == taggedTokens[idx]))
                {
                    unManagedKeywords.Add(tokens[idx]);
                }
            }

            if (scoredKeywords == null)
            {
                scoredKeywords = new Dictionary<string, double>();
            }
            
            foreach (string word in unManagedKeywords)
            {
                if (scoredKeywords.ContainsKey(word))
                {
                    scoredKeywords[word] = scoredKeywords[word] + 1.0;
                }
                else
                {
                    scoredKeywords.Add(word, 0.01);
                }
            }


            return scoredKeywords;
        }
        public static List<string> Words(string statement)
        {
            return statement.TokenizeStatement()
                .ToList();
        }
        public static List<string> Pos(List<string> words)
        {
            return words
                .ToArray()
                .TagPartsOfSpeech()
                .ToList();
        }
    }
}
