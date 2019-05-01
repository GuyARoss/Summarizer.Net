using System.Collections.Generic;

namespace Summarizer.Core
{
    public interface ISummarizationLayer
    {
        Dictionary<string, double> Invoke(string statement);
        Dictionary<string, double> Invoke(Dictionary<string, double> scoredStatements);
    }
}
