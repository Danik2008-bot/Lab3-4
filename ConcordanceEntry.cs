namespace TextProcessing;

public class ConcordanceEntry
{
    public int Count { get; set; }

    public SortedSet<int> Lines { get; } = new();
}
