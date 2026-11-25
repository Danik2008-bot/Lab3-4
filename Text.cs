using System.Xml.Serialization;

namespace TextProcessing;

public partial class Text
{
    [XmlElement("Sentence")]
    public List<Sentence> Sentences { get; set; } = new();

    public override string ToString() =>
        string.Join(" ", Sentences.Select(s => s.ToString()));
}
