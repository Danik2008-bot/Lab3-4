namespace TextProcessing;

public partial class Text
{
    public Dictionary<string, ConcordanceEntry> BuildConcordance()
    {
        var result = new Dictionary<string, ConcordanceEntry>();

        foreach (Word w in Sentences.SelectMany(s => s.Words))
        {
            string key = w.Value.ToLowerInvariant();

            if (!result.TryGetValue(key, out ConcordanceEntry? entry))
            {
                entry = new ConcordanceEntry();
                result[key] = entry;
            }

            entry.Count++;
            entry.Lines.Add(w.Line);
        }

        return result;
    }
}
