namespace TextProcessing;

public partial class Text
{
    // Project04. Строим конкорданс: слово -> (сколько раз, в каких строках).
    // Dictionary даёт быстрый поиск слова, SortedSet в записи - уникальные отсортированные номера строк.
    public Dictionary<string, ConcordanceEntry> BuildConcordance()
    {
        var result = new Dictionary<string, ConcordanceEntry>();

        foreach (Word w in Sentences.SelectMany(s => s.Words))
        {
            string key = w.Value.ToLowerInvariant(); // "A" и "a" - одно слово

            if (!result.TryGetValue(key, out ConcordanceEntry? entry))
            {
                entry = new ConcordanceEntry();
                result[key] = entry;
            }

            entry.Count++;
            entry.Lines.Add(w.Line); // повтор номера строки SortedSet игнорирует сам
        }

        return result;
    }
}
