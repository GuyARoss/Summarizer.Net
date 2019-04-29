using System;
using System.Collections.Generic;

namespace Summarizer.Core.KeywordExtractors
{
    public class RandomSelectionExtractor : IKeywordExtractor
    {
        public readonly uint MaxKeywords;
        public readonly double SelectionLikelihood; // value between 0.0 - 1;

        private readonly Random _random = new Random();

        public RandomSelectionExtractor()
        {
            MaxKeywords = 5;
            SelectionLikelihood = 0.4;
        }
        public RandomSelectionExtractor(uint maxKeywords) : this()
        {
            MaxKeywords = maxKeywords;
        }
        public RandomSelectionExtractor(double selectionLikelihood) : this()
        {
            SelectionLikelihood = selectionLikelihood;
        }
        public RandomSelectionExtractor(int maxKeywords, double selectionLikelihood) : this(maxKeywords)
        {
            SelectionLikelihood = selectionLikelihood;
        }

        public Dictionary<string, double> Invoke(string statement)
        {
            string[] words = statement.Split(' ');
            var resolvedExtractor = new Dictionary<string, double>();

            foreach (string word in words)
            {
                if (resolvedExtractor.Count >= MaxKeywords)
                {
                    break;
                }

                double seed = _random.NextDouble();
                if (seed <= SelectionLikelihood)
                {
                    resolvedExtractor.Add(word, seed);
                }
            }

            return resolvedExtractor;
        }
        public Dictionary<string, double> Invoke(Dictionary<string, double> scoredStatements)
        {
            uint generationLength = 0;
            foreach (var scoredStatement in scoredStatements)
            {
                if (generationLength >= MaxKeywords)
                {
                    break;
                }

                double seed = _random.NextDouble();
                if (seed <= SelectionLikelihood)
                {
                    generationLength++;

                    scoredStatements.Remove(scoredStatement.Key);
                    scoredStatements.Add(scoredStatement.Key, scoredStatement.Value + seed);                   
                }
            }

            return scoredStatements;
        }
    }
}
