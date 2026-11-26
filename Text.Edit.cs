namespace TextProcessing;

public partial class Text
{
    private const string Consonants = "bcdfghjklmnpqrstvwxyzбвгджзйклмнпрстфхцчшщ";

    private static bool StartsWithConsonant(string word) =>
        word.Length > 0 && Consonants.Contains(char.ToLowerInvariant(word[0]));
    public void RemoveWordsStartingWithConsonant(int length)
    {
        foreach (Sentence s in Sentences)
        {
            s.Tokens.RemoveAll(t =>
                t is Word w && w.Length == length && StartsWithConsonant(w.Value));
        }
        RemoveEmptySentences();
    }
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
    public void RemoveStopWords(HashSet<string> stopWords)
    {
        foreach (Sentence s in Sentences)
        {
            s.Tokens.RemoveAll(t => t is Word w && stopWords.Contains(w.Value));
        }
        RemoveEmptySentences();
    }
    private void RemoveEmptySentences() =>
        Sentences.RemoveAll(s => !s.Words.Any());
}
