using System.Collections.Generic;

namespace Summarizer.Core
{
    public interface IKeywordExtractor
    {
        /// <summary>
        /// Invoke with the base statement.
        /// </summary>
        /// <param name="statement"></param>
        /// <returns>vectorized keywords</returns>
        Dictionary<string, double> Invoke(string statement);

        /// <summary>
        /// Invoke with vectorized statements.
        /// </summary>
        /// <param name="scoredStatements"></param>
        /// <returns>newly vectorized statements</returns>
        Dictionary<string, double> Invoke(Dictionary<string, double> scoredStatements);
    }
}
