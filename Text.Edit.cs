namespace TextProcessing;

public partial class Text
{
    private const string Consonants = "bcdfghjklmnpqrstvwxyzбвгджзйклмнпрстфхцчшщ";

    private static bool StartsWithConsonant(string word) =>
        word.Length > 0 && Consonants.Contains(char.ToLowerInvariant(word[0]));

    // Операция 4. Удалить слова заданной длины, начинающиеся с согласной.
    public void RemoveWordsStartingWithConsonant(int length)
    {
        foreach (Sentence s in Sentences)
        {
            s.Tokens.RemoveAll(t =>
                t is Word w && w.Length == length && StartsWithConsonant(w.Value));
        }
        RemoveEmptySentences();
    }

    // Операция 5. В предложении с номером sentenceIndex (с 0) заменить слова длины length на replacement.
    // Длина replacement может быть любой - просто меняем Value.
    // Возвращает количество замен; -1, если такого предложения нет.
    public int ReplaceWordsInSentence(int sentenceIndex, int length, string replacement)
    {
        if (sentenceIndex < 0 || sentenceIndex >= Sentences.Count)
            return -1;

        int count = 0;
        foreach (Word w in Sentences[sentenceIndex].Words)
        {
            if (w.Length == length)
            {
                w.Value = replacement;
                count++;
            }
        }
        return count;
    }

    // Операция 6. Удалить стоп-слова. HashSet даёт поиск за O(1).
    public void RemoveStopWords(HashSet<string> stopWords)
    {
        foreach (Sentence s in Sentences)
        {
            s.Tokens.RemoveAll(t => t is Word w && stopWords.Contains(w.Value));
        }
        RemoveEmptySentences();
    }

    // Если в предложении не осталось слов (только знаки) - выбрасываем его целиком.
    private void RemoveEmptySentences() =>
        Sentences.RemoveAll(s => !s.Words.Any());
}
