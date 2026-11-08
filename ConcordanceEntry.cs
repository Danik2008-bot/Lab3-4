namespace TextProcessing;

/// <summary>Запись конкорданса для одного слова: сколько раз встретилось и в каких строках.</summary>
public class ConcordanceEntry
{
    public int Count { get; set; }

    // SortedSet: номера строк не повторяются и всегда идут по возрастанию.
    public SortedSet<int> Lines { get; } = new();
}
