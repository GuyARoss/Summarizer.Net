using OpenNLP.Tools.Tokenize;
using OpenNLP.Tools.PosTagger;

namespace Summarizer.Infrastructure.Nlp
{
    public static class Extensions
    {
        private static readonly string _englishPOSPath = "../nlp/EnglishPOs.nbin";
        private static readonly string _englishTagDictPath = "../nlp/tagdict";

        public static string[] TokenizeStatement(this string statement)
        {
            var tokenizer = new EnglishRuleBasedTokenizer(false);
            return tokenizer.Tokenize(statement);
        }
        public static string[] TagPartsOfSpeech(this string[] tokens)
        {
            var posTagger = new EnglishMaximumEntropyPosTagger(_englishPOSPath, _englishTagDictPath);
            return posTagger.Tag(tokens);
        }

        public static string ToStringPos(this PosType tagType)
        {
            switch (tagType)
            {
                case PosType.CC:
                    return "CC";
                case PosType.CD:
                    return "CD";
                case PosType.DT:
                    return "DT";
                case PosType.EX:
                    return "EX";
                case PosType.FW:
                    return "FW";
                case PosType.IN:
                    return "IN";
                case PosType.JJ:
                    return "JJ";
                case PosType.JJR:
                    return "JJR";
                case PosType.JJS:
                    return "JJS";
                case PosType.LS:
                    return "LS";
                case PosType.MD:
                    return "MD";
                case PosType.NN:
                    return "NN";
                case PosType.NNP:
                    return "NNP";
                case PosType.NNPS:
                    return "NNPS";
                case PosType.NNS:
                    return "NNS";
                case PosType.PDT:
                    return "PDT";
                case PosType.POS:
                    return "POS";
                case PosType.PRP:
                    return "PRP";
                case PosType.PRPS:
                    return "PRP$";
                case PosType.RB:
                    return "RB";
                case PosType.RBR:
                    return "RBR";
                case PosType.RBS:
                    return "RBS";
                case PosType.RP:
                    return "RP";
                case PosType.SYM:
                    return "SYM";
                case PosType.TO:
                    return "TO";
                case PosType.UH:
                    return "UH";
                case PosType.VB:
                    return "VB";
                case PosType.VBD:
                    return "VBD";
                case PosType.VBG:
                    return "VBG";
                case PosType.VBN:
                    return "VBN";
                case PosType.VBP:
                    return "VBP";
                case PosType.VBZ:
                    return "VBZ";
                case PosType.WDT:
                    return "WDT";
                case PosType.WP:
                    return "WP";
                case PosType.WPS:
                    return "WP$";
                case PosType.WRB:
                    return "WRB";
                case PosType.WTF:
                    return ",";
                case PosType.WTF_TWO:
                    return ";";
            }
            return null;
        }        
        public static PosType ToPos(this string pos)
        {
            switch (pos)
            {
                case "CC":
                    return PosType.CC;
                case "CD":
                    return PosType.CD;
                case "DT":
                    return PosType.DT;
                case "EX":
                    return PosType.EX;
                case "FW":
                    return PosType.FW;
                case "IN":
                    return PosType.IN;
                case "JJ":
                    return PosType.JJ;
                case "JJR":
                    return PosType.JJR;
                case "JJS":
                    return PosType.JJS;
                case "LS":
                    return PosType.LS;
                case "MD":
                    return PosType.MD;
                case "NN":
                    return PosType.NN;
                case "NNP":
                    return PosType.NNP;
                case "NNPS":
                    return PosType.NNPS;
                case "NNS":
                    return PosType.NNS;
                case "PDT":
                    return PosType.PDT;
                case "POS":
                    return PosType.POS;
                case "PRP":
                    return PosType.PRP;
                case "PRP$":
                    return PosType.PRPS;
                case "RB":
                    return PosType.RB;
                case "RBR":
                    return PosType.RBR;
                case "RBS":
                    return PosType.RBS;
                case "RP":
                    return PosType.RP;
                case "SYM":
                    return PosType.SYM;
                case "TO":
                    return PosType.TO;
                case "UH":
                    return PosType.UH;
                case "VB":
                    return PosType.VB;
                case "VBD":
                    return PosType.VBD;
                case "VBG":
                    return PosType.VBG;
                case "VBN":
                    return PosType.VBN;
                case "VBP":
                    return PosType.VBP;
                case "VBZ":
                    return PosType.VBZ;
                case "WDT":
                    return PosType.WDT;
                case "WP":
                    return PosType.WP;
                case "WP$":
                    return PosType.WPS;
                case "WRB":
                    return PosType.WRB;
                case ",":
                    return PosType.WTF;
                case ";":
                    return PosType.WTF_TWO;
            }

            return PosType.WTF_TWO;
        }
    }    
}
