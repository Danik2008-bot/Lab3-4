using System.Text;
using System.Xml.Serialization;

namespace TextProcessing;

/// <summary>Предложение: коллекция слов и знаков препинания в порядке следования.</summary>
public class Sentence
{
    // Один список хранит и Word, и Punctuation. Атрибуты XmlElement говорят сериализатору,
    // как называть элементы в XML для каждого типа токена.
    [XmlElement("Word", typeof(Word))]
    [XmlElement("Punctuation", typeof(Punctuation))]
    public List<Token> Tokens { get; set; } = new();

    [XmlIgnore]
    public IEnumerable<Word> Words => Tokens.OfType<Word>();

    [XmlIgnore]
    public int WordCount => Words.Count();

    /// <summary>Длина предложения в символах (считаем по тексту, как оно выводится).</summary>
    [XmlIgnore]
    public int Length => ToString().Length;

    /// <summary>Вопросительное: среди завершающих знаков (. ! ? …) есть "?".</summary>
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
        bool noSpaceBefore = true; // в начале предложения пробел не нужен

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
                noSpaceBefore = true; // после открывающей скобки пробела нет
            }
            else
            {
                sb.Append(t.Value); // обычная пунктуация клеится к предыдущему слову
                noSpaceBefore = false;
            }
        }
        return sb.ToString();
    }
}
