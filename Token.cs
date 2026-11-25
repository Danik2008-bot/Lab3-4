using System.Xml.Serialization;

namespace TextProcessing;

public abstract class Token
{
    public string Value { get; set; } = "";

    public override string ToString() => Value;
}
public class Word : Token
{
    public int Line { get; set; }

    [XmlIgnore]
    public int Length => Value.Length;


    public Word() { }

    public Word(string value, int line)
    {
        Value = value;
        Line = line;
    }
}

public class Punctuation : Token
{
    [XmlIgnore]
    public bool IsSentenceEnd => Value is "." or "!" or "?" or "…";

    public Punctuation() { }

    public Punctuation(string value)
    {
        Value = value;
    }
}
