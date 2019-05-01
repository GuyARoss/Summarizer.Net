## Summarizer .Net 
Summarizer .Net is a library for performing high-level sentence summarization, that features Natural Language Processing in .Net that makes sentence summarization extremely easy and versatile. 

Extracted for public use from [Project Orva](https://www.ross-cdn.com/orva).


## Installation

#### Basic Usage Installation
Import all of the Summarizer Dlls and external dependencies from the [distribute directory](./distribute) into your .net project solution. 
- Summarizer.Core
- Summarizer.Infrastructure
- Newtonsoft.Json
- OpenNLPp

#### Synoym-izer Infrastructure Dependency
The built in synonymizer uses the [Merriam-Webster Dictionary Api](https://www.dictionaryapi.com/). To make use of the api, it is required that you either inject your api key via the synonymizer class constructor (view example below) or rebuilding the `Summarizer.Infrastructure` solution to dll with your api key.

```c#
using Summarizer.Core;
using Summarizer.Core.Summarizers;
using Summarizer.Infrastruture.Dictionary;

// ...
string key = "some-key"; // your api key
var synonymizerSummarizer = new Synonymizer(new DictionaryApi(key)); // injecting the dictionary api
```
Or in the case that an extractor dependency also needs to be injected.
```c#
// ...
string key = "some-key"; // your api key
ISummarizationLayer extractor; // your extractor ependency

var synonymizerSummarizer = new Synonymizer(extractor, new DictionaryApi(key)); // injecting the dictionary with an extractor.
```

#### Nlp Resources
To make use of the nlp operations provided by the .net summarizer, extract the `NLP` contents within the [project resources](./resources/NLP) directory and place it within a `nlp` directory within your project's bin (per the directories listed in the NLP extensions `Infrastructure.Nlp` file).

## Examples

### Summarizers
----------------------------------
### Text Rank
Uses the [TextRank algorithm](https://web.eecs.umich.edu/~mihalcea/papers/mihalcea.emnlp04.pdf) to vectorize keywords in order by frequency with respect to the vectorization to summarize text.

#### Basic Usage

```c#
using Summarizer.Core;
using Summarizer.Core.Summarizers;

// ...
string text; // statement(s) to be summarized

ISummarizationLayer textRankSummarizer = new TextRankByFrequency();
string summarizedText = SummarizationHandler(textRankSummarizer).Invoke(text); // summarized text.

```

#### NLP Usage
Applying a NLP model to the TextRank algorithm only requires an injection of the NLP keyword extractor into the summarization instance. In this particular case, we are using a version of the NLP extractor designed for extraction based off of frequency from the default TextRank keyword vectorization method.

```c#
using Summarizer.Core;
using Summarizer.Core.Summarizers;
using Summarizer.Core.KeywordExtractors;

// ... 
string text; // statement(s) to be summarized

var nlpKeywordExtractor = new NlpFrequencyExtractor();

ISummarizationLayer nlpTextRankSummarizer = new TextRankByFrequency(nlpKeywordExtractor);
string summarizedText = SummarizationHandler(nlpTextRankSummarizer).Invoke(text); // summarized text.

```

### Synonymizer
Uses a synonym based algorithm to replace vectorized keywords into new words, while maintaining the placement of non-vectorized keywords. 

#### Basic Usage

```c#
using Summarizer.Core;
using Summarizer.Core.Summarizers; 

// ...
string text; // statement(s) to be summarized
ISummarizationLayer synonymSummarizer = new Synonymizer();

string summarizedText = SummarizationHandler(synonymSummarizer).Invoke(text);

```

### Node Proximity
Allows the vectorization to maintain the original sentence order. It is recommend to use this summarizer in conjunction with other summarizers as a post-summarization method.

#### Basic Usage

```c#
using Summarizer.Core;
using Summarizer.Core.Summarizers;

string text; // statement(s) to be summarized
ISummarizationLayer nodeProximitySummarizer = new NodeProximity(text); // requires the text to differ the vectorized indices.

string summarizedText = SummarizationHandler(nodeProximitySummarizer).Invoke(text);

```

#### Adjusting the Rating Weight
The node proximity summarizer has support for changing how high it should score statement velocity based off of how far both parent & child statement nodes are from the original index structure. 

__Note__: Higher values will saturate all other summarization methods.The recommended range is between 0.2-3

```c#
using Summarizer.Core;
using Summarizer.Core.Summarizers;

string text; // statement(s) to be summarized.
double ratingRate = 1.4;
ISummarizationLayer nodeProximitySummarizer = new NodeProximity(text, ratingWeight); // initialize weight in the constructor.

string summarizedText = SummarizationHandler(nodeProximitySummarizer).Invoke(text);

```

### Usage of Multiple Summarization Methods
Support for layering summarization methods.

```c#
using Summarizer.Core;
using Summarizer.Core.Summarizer;

// ...
string text; // statement(s) to be summarized

ISummarizationLayer layerOne = TextRankByFrequency();
ISummarizationLayer layerTwo = Synonymizer();

string summarizedText = new SummarizationHandler(new List<ISummarizationLayer> {
  layerOne,
  layerTwo
}).Invoke(text);
```

### Keyword Extractors
-----------------------------------
Keyword extractors provide injectable keyword extraction/ vectorization methods to summarization classes that extend from ` ISummarizationLayer ` as well as expose a injectable constructor field for `IKeywordExtractor`.


### NLP Frequency Extractor
Vectorize keywords from how frequent a keyword appears in a statement with Natural Langauge Processing.

```c#
using Summarizer.Core;
using Summarizer.Core.Summarizers;
using Summarizer.Core.KeywordExtractors;

// ...
string text; // statement(s) to be summarized
IKeywordExtractor extractor = NlpFrequencyExtractor();

ISummarizationLayer summarizer = new TextRankByFrequency(extractor); // initialize with any ISummarizationLayer instance 

string summarizedText = new SummarizationHandler(summarizer).Invoke(text);
```

Additionally, the NLP Frequency Extractor has support for injectable omitted parts of speech. In the case of requiring an adjusted omitted list of NLP parts of speech. 

- [Available Parts of Speech Tags](https://www.ling.upenn.edu/courses/Fall_2003/ling001/penn_treebank_pos.html)


#### Example of Overwriting the Omission Property 
```c#
using Summarizer.Core.KeywordExtractors;
using Summarizer.Core.Infrastructure.Nlp;

// ...
var ommitedPartsOfSpeech = new List<PosType> {
  PosType.CC
};
var nlpFrequencyExtractor = new NlpFrequencyExtractor(omittedPartsOfSpeech);

var summarizer = new SummarizerInstance(nlpFrequencyExtractor); // any instance of ISummarizationLayer
// ...
```
__Note__: The NLP Frequency extractor comes preconfigured with an omitted list of Parts of Speech
```
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
```

### Regular Frequency Extractor
Vectorize keywords similar to the NLP frequency extractor without NLP dependencies. 

```c#
using Summarizer.Core;
using Summarizer.Core.Summarizers;
using Summarizer.Core.KeywordExtractors;

// ...
string text; // statement(s) to be summarized
IKeywordExtractor extractor = RegularFrequencyExtractor();

ISummarizationLayer summarizer = new TextRankByFrequency(extractor); // initialize with any ISummarizationLayer instance 

string summarizedText = new SummarizationHandler(summarizer).Invoke(text);
```

### Random Selection Extractor
Vectorize keywords based off a random seed.

```c#
using Summarizer.Core;
using Summarizer.Core.Summarizers;
using Summarizer.Core.KeywordExtractors;

// ...
string text; // statement(s) to be summarized
IKeywordExtractor extractor = new RandomSelectionExtractor();

ISummarizationLayer summarizer = new TextRankByFrequency(extractor); // initialize with any ISummarizationLayer instance 

string summarizedText = new SummarizationHandler(summarizer).Invoke(text);
```

The random selection extractor also has support for an additional weight `selection likelihood` which ranges from `0.0` and `1.0`. This selection likelihood denotes how likely a keyword is to be selected. 

The `RandomSelectionExtractor` as well has support for a maximum keyword amount. 

```c#
using Summarizer.Core.KeywordExtractors;

// ...
/**
Default Max Size: 5
Default Selection Likelihood: 0.4
**/
IKeywordExtractor randomExtractor = new RandomSelectionExtractor(); // sets defaults
IKeywordExtractor randomExtractorWithMaxSize = new RandomSelectionExtractor(5); // sets the max size to 5
IKeywordExtractor randomExtractorWithLikelihood = new RandomSelectionExtractor(0.3); // sets the selectionLikelihood to .3
IKeywordExtractor randomExtractorWithBoth = new RandomSelectionExtractor(5, 0.3); // sets both
```

### Applying Abstract Intensity
Extractor instances that inherit directly from both `IKeywordExtractor` and the abstract class `ExtractionProvider` allow the provision of an abstract intensity value. This abstract intensity weight controls the amount of vectorized keywords generated by the extractor. Weight range is between `0.0` and `1.0`. The default value is `0.4`.

```c#
using Summarizer.Core.KeywordExtractors;

IKeywordExtractor extractor = new SupportedExtractor(0.5); // applies intensity to support extractor
```

#### Supported Extractors
- [Nlp Frequency Extractor](#Nlp-Frequency-Extractor)
- [Regular Frequency Extractor](#Regular-Frequency-Extractor)

## Contributing
Feel free to contribute by opening a Pull Request or an issue thread.

Contributions are always appreciated! 

## License
Summarizer .Net is [MIT licensed](./LICENSE)
