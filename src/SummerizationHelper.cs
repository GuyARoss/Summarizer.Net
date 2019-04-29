using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Summarizer.Core
{
    public static class SummerizationHelper
    {
        public static List<string> ConvertStatementToSentences(string statement)
        {
            return Regex.Split(statement, @"(?<=[\.!\?])\s+")
            .ToList();
        }
    }
}
