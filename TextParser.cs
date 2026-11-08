using System.Text;
using System.Text.RegularExpressions;

namespace TextProcessing;

/// <summary>
/// Парсер 2 из задания: читаем файл построчно и режем строки на токены регулярным выражением.
/// </summary>
public static class TextParser
{
    // Альтернатива 1: слово = буквы/цифры, внутри допускаются ' ’ - (don't, кто-то).
    // Альтернатива 2: любой другой непробельный символ = знак препинания.
    private static readonly Regex TokenRegex =
        new(@"[\p{L}\p{N}]+(?:['’\-][\p{L}\p{N}]+)*|\S", RegexOptions.Compiled);

    private const string Closers = ")]}»”\"'";

    public static Text ParseFile(string path) =>
        Parse(File.ReadAllLines(path, Encoding.UTF8));

    public static Text Parse(IEnumerable<string> lines)
    {
        var text = new Text();
        var current = new Sentence();
        bool ended = false; // уже встретили . ! ? … и ждём, продолжится ли предложение
        int lineNo = 0;

        void Flush()
        {
            if (current.Tokens.Count > 0)
                text.Sentences.Add(current);
            current = new Sentence();
            ended = false;
        }

        foreach (string line in lines)
        {
            lineNo++;

            foreach (Match m in TokenRegex.Matches(line))
            {
                string v = m.Value;

                if (char.IsLetterOrDigit(v[0]))
                {
                    if (ended) Flush();                 // после точки началось новое предложение
                    current.Tokens.Add(new Word(v, lineNo));
                }
                else
                {
                    var p = new Punctuation(v);

                    // "?!" и "..." остаются в одном предложении; закрывающая кавычка тоже.
                    if (ended && !p.IsSentenceEnd && !Closers.Contains(v))
                        Flush();

                    current.Tokens.Add(p);
                    if (p.IsSentenceEnd) ended = true;
                }
            }
        }

        Flush(); // последнее предложение (если текст не заканчивается точкой)
        return text;
    }
}
