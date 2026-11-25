using System.Text;
using System.Xml.Serialization;

namespace TextProcessing;

public class Sentence
{
    [XmlElement("Word", typeof(Word))]
    [XmlElement("Punctuation", typeof(Punctuation))]
    public List<Token> Tokens { get; set; } = new();

    [XmlIgnore]
    public IEnumerable<Word> Words => Tokens.OfType<Word>();

    [XmlIgnore]
    public int WordCount => Words.Count();

    [XmlIgnore]
    public int Length => ToString().Length;

    [XmlIgnore]
    public bool IsQuestion
    {
        get
        {
            for (int i = Tokens.Count - 1; i >= 0; i--)
            {
                if (Tokens[i] is not Punctuation p || !p.IsSentenceEnd)
                    break;
                if (p.Value == "?")
                    return true;
            }
            return false;
        }
    }

    private const string Openers = "([{«„";

    public override string ToString()
    {
        var sb = new StringBuilder();
        bool noSpaceBefore = true;

        foreach (Token t in Tokens)
        {
            if (t is Word)
            {
                if (!noSpaceBefore) sb.Append(' ');
                sb.Append(t.Value);
                noSpaceBefore = false;
            }
            else if (Openers.Contains(t.Value))
            {
                if (!noSpaceBefore) sb.Append(' ');
                sb.Append(t.Value);
                noSpaceBefore = true;
            }
            else
            {
                sb.Append(t.Value);
                noSpaceBefore = false;
            }
        }
        return sb.ToString();
    }
}
