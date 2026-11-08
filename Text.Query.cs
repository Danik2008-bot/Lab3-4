namespace TextProcessing;

public partial class Text
{
    // Операция 1. Предложения по возрастанию количества слов.
    // OrderBy в LINQ устойчивый: предложения с равным числом слов остаются в исходном порядке.
    public IEnumerable<Sentence> SentencesByWordCount() =>
        Sentences.OrderBy(s => s.WordCount);

    // Операция 2. Предложения по возрастанию длины (в символах).
    public IEnumerable<Sentence> SentencesByLength() =>
        Sentences.OrderBy(s => s.Length);

    // Операция 3. В вопросительных предложениях найти слова заданной длины без повторов.
    // Приводим к нижнему регистру, чтобы "Book" и "book" считались одним словом.
    public IEnumerable<string> WordsInQuestions(int length) =>
        Sentences
            .Where(s => s.IsQuestion)
            .SelectMany(s => s.Words)
            .Where(w => w.Length == length)
            .Select(w => w.Value.ToLowerInvariant())
            .Distinct();
}
