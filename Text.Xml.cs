using System.Xml.Serialization;

namespace TextProcessing;

public partial class Text
{
    public void ExportToXml(string path)
    {
        var serializer = new XmlSerializer(typeof(Text));
        using var stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, this);
    }
}
