using System;
using System.Linq;
using System.Collections.Generic;

namespace Summarizer.Core.Summarizers
{
    public class NodeProximity : ISummarizationLayer
    {
        public readonly string InitialStatement;
        public readonly double VectorWeight;

        public NodeProximity(string statement)
        {
            InitialStatement = statement;
            VectorWeight = 0.5;
        }
        public NodeProximity(string statement, double vectorWeight) : this(statement)
        {
            VectorWeight = vectorWeight;
        }

        public Dictionary<string, double> Invoke(string statement)
        {
            var unscoredStatements = _initializeEmpty(statement);
            return TagByProximity(unscoredStatements);
        }

        public Dictionary<string, double> Invoke(Dictionary<string, double> scoredStatements)
        {
            return TagByProximity(scoredStatements);
        }

        protected Dictionary<string, double> TagByProximity(Dictionary<string, double> scoredStatements)
        {
            scoredStatements = scoredStatements
                .OrderBy(x => x.Value)
                .Reverse()
                .ToDictionary(x => x.Key, x => x.Value);

            var proximityScoredStatements = new Dictionary<string, double>();
            var originalSentenceOrder = SummarizationHelper.ConvertStatementToSentences(InitialStatement);

            var keys = scoredStatements.Keys.ToList();
            var values = scoredStatements.Values.ToList();
            for (int idx = 0; idx < scoredStatements.Count; idx++)
            {
                int currentStatementIndex = 0;
                int parentIndex = scoredStatements.Count;
                int childIndex = 0;

                for (int originalIdx = 0; originalIdx < originalSentenceOrder.Count; originalIdx++)
                {
                    string currentStatement = originalSentenceOrder[originalIdx];
                    string parentStatement = (originalIdx == 0) ? null : originalSentenceOrder[originalIdx - 1]; // parent is it's self if root node.
                    string childStatement = (originalIdx == originalSentenceOrder.Count - 1) ? null: originalSentenceOrder[originalIdx + 1]; // child is it's self if last node

                    if (parentStatement == null)
                    {
                        parentIndex = 0;
                    } else if (childStatement == null)
                    {
                        childIndex = originalSentenceOrder.Count - 1;
                    }                    

                    if (keys[idx] == currentStatement)
                    {
                        currentStatementIndex = originalIdx;
                    }
                    else if (keys[idx] == parentStatement)
                    {
                        parentIndex = originalIdx;
                    }
                    else if (keys[idx] == childStatement)
                    {
                        childIndex = originalIdx;
                    }
                }

                int distanceFromParent = Math.Abs(currentStatementIndex - parentIndex);
                int distanceFromChild = Math.Abs(currentStatementIndex - childIndex);

                double parentScore = _ratedScore(VectorWeight, distanceFromParent, keys.Count);
                double childScore = _ratedScore(VectorWeight, distanceFromChild, keys.Count);

                proximityScoredStatements.Add(keys[idx], values[idx] + (parentScore + childScore) * Math.Abs(currentStatementIndex - idx + 1));
            }

            return proximityScoredStatements;
        }
        private static Dictionary<string, double> _initializeEmpty(string statement)
        {
            var baseStatements = new Dictionary<string, double>();
            List<string> sentences = SummarizationHelper.ConvertStatementToSentences(statement);

            foreach (string sentence in sentences)
            {
                baseStatements.Add(sentence, 0);
            }

            return baseStatements;
        }

        private static double _ratedScore(double weight, int currentDistance, int maxDistance)
        {
            return Math.Pow(currentDistance, (currentDistance * (weight / maxDistance)));
        }
    }
}
