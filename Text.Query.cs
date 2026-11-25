namespace TextProcessing;

public partial class Text
{
    public IEnumerable<Sentence> SentencesByWordCount() =>
        Sentences.OrderBy(s => s.WordCount);
    public IEnumerable<Sentence> SentencesByLength() =>
        Sentences.OrderBy(s => s.Length);

    public IEnumerable<string> WordsInQuestions(int length) =>
        Sentences
            .Where(s => s.IsQuestion)
            .SelectMany(s => s.Words)
            .Where(w => w.Length == length)
            .Select(w => w.Value.ToLowerInvariant())
            .Distinct();
}
