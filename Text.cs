using System.Xml.Serialization;

namespace TextProcessing;

/// <summary>
/// Текст: коллекция предложений. Класс разбит на несколько файлов (partial):
///   Text.cs            - данные и вывод
///   Text.Query.cs      - операции 1-3 (сортировка, поиск)
///   Text.Edit.cs       - операции 4-6 (изменение текста)
///   Text.Xml.cs        - операция 7 (экспорт в XML)
///   Text.Concordance.cs - Project04 (конкорданс)
/// </summary>
public partial class Text
{
    [XmlElement("Sentence")]
    public List<Sentence> Sentences { get; set; } = new();

    public override string ToString() =>
        string.Join(" ", Sentences.Select(s => s.ToString()));
}
