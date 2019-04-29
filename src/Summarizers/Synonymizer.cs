using System;
using System.Collections.Generic;
using System.Linq;

using Summarizer.Infrastructure.Dictionary;
using Summarizer.Infrastructure.Dictionary.Entities;

using Summarizer.Infrastructure.Nlp;

namespace Summarizer.Core.Summarizers
{
    public class Synonymizer : ISummarizationLayer
    {
        public readonly IKeywordExtractor KeywordExtractor;
        public readonly DictionaryApi Api;

        private readonly Random _rnd = new Random();
        
        public Synonymizer(IKeywordExtractor extractor)
        {
            KeywordExtractor = extractor;
            Api = new DictionaryApi();
        }
        public Synonymizer(IKeywordExtractor extractor, DictionaryApi api) : this(extractor)
        {
            Api = api;
        }

        public Dictionary<string, double> Invoke(Dictionary<string, double> statements)
        {
            return _synonymize(statements);
        }
        public Dictionary<string, double> Invoke(string statement)
        {            
            var sentences = SummerizationHelper.ConvertStatementToSentences(statement);

            var emptyInitializedSentences = sentences
                .Select(x => new KeyValuePair<string, double>(x, 0.0))
                .ToDictionary(x => x.Key, x => x.Value);

            return _synonymize(emptyInitializedSentences);
        }

        private string SelectRandomSynonym(IEnumerable<List<string>> syns)
        {
            var flatSyns = syns
                .SelectMany(i => i)
                .ToList();

            int rndNum = _rnd.Next(0, flatSyns.Count);

            return flatSyns[rndNum];
        }
        private Dictionary<string, double> _synonymize(Dictionary<string, double> scoredStatements)
        {
            var updatedScoredStatements = new Dictionary<string, double>();

            var keywords = KeywordExtractor.Invoke(string.Join(" ", scoredStatements.Select(x => x.Key)));

            foreach (var sentence in scoredStatements)
            {
                var tokens = NLPExtractor.Words(sentence.Key);
                var pos = NLPExtractor.Pos(tokens);

                string recontructedStatement = "";
                double score = sentence.Value;

                for (int idx = 0; idx < tokens.Count; idx++)
                {
                    string word = tokens[idx];

                    if (keywords.Keys.Contains(word))
                    {
                        var wordEntries = Api.Invoke(tokens[idx]);
                        wordEntries.Wait();

                        if (wordEntries != null)
                        {
                            List<DictionaryEntity> results = wordEntries.Result;
                            if (results != null && results.Count > 0)
                            {
                                string currentFunctionalLabel = MapPosToFunctionalLabel(pos[idx].ToPos());

                                if (currentFunctionalLabel != null)
                                {
                                    DictionaryEntity correctDataset = results
                                        .Find(x => x.Fl == currentFunctionalLabel);

                                    if (correctDataset != null)
                                    {
                                        word = SelectRandomSynonym(correctDataset.Meta.Syns);
                                        score += 0.1;
                                    }
                                }
                            }
                        }
                    }
                    
                    recontructedStatement += string.Format("{0} ", word);
                }

                updatedScoredStatements.Add(recontructedStatement, score);
            }

            return scoredStatements;
        }

        private static string MapPosToFunctionalLabel(PosType partOfSpeech)
        {
            switch(partOfSpeech)
            {
                case PosType.JJ:
                    return "adjective";
                case PosType.RB:
                    return "adverb";
                case PosType.NN:
                    return "noun";
                case PosType.VB:
                    return "verb";
            }

            return null;
        }
    }
}
