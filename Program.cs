using System.Globalization;
using System.Text;

namespace TextProcessing;

internal static class Program
{
    private static Text _text = new();
    private static string _path = "";
    private static string _dataDir = "";

    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        _dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
        _path = args.Length > 0 ? args[0] : Path.Combine(_dataDir, "sample_en.txt");

        if (!File.Exists(_path))
        {
            Console.WriteLine($"Файл не найден: {_path}");
            return;
        }

        LoadText();

        while (true)
        {
            PrintMenu();
            string? cmd = Console.ReadLine()?.Trim();
            Console.WriteLine();

            switch (cmd)
            {
                case "1": PrintSentences(_text.SentencesByWordCount()); break;
                case "2": PrintSentences(_text.SentencesByLength()); break;
                case "3": FindWordsInQuestions(); break;
                case "4": RemoveConsonantWords(); break;
                case "5": ReplaceWords(); break;
                case "6": RemoveStopWords(); break;
                case "7": ExportXml(); break;
                case "8": PrintConcordance(); break;
                case "9": LoadText(); break;
                case "0": return;
                default: Console.WriteLine("Неизвестная команда."); break;
            }
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== Обработка текста ===");
        Console.WriteLine("1 - Предложения по возрастанию числа слов");
        Console.WriteLine("2 - Предложения по возрастанию длины");
        Console.WriteLine("3 - Слова заданной длины в вопросительных предложениях");
        Console.WriteLine("4 - Удалить слова заданной длины, начинающиеся с согласной");
        Console.WriteLine("5 - Заменить слова заданной длины в предложении");
        Console.WriteLine("6 - Удалить стоп-слова");
        Console.WriteLine("7 - Экспорт в XML");
        Console.WriteLine("8 - Конкорданс (Project04)");
        Console.WriteLine("9 - Загрузить текст заново (отменить изменения)");
        Console.WriteLine("0 - Выход");
        Console.Write("> ");
    }

    private static void LoadText()
    {
        _text = TextParser.ParseFile(_path);
        Console.WriteLine($"Загружен файл {_path}: предложений - {_text.Sentences.Count}.");
    }

    private static void PrintSentences(IEnumerable<Sentence> sentences)
    {
        foreach (Sentence s in sentences)
            Console.WriteLine($"[слов: {s.WordCount}, длина: {s.Length}] {s}");
    }

    private static void FindWordsInQuestions()
    {
        int len = ReadInt("Длина слова: ");
        var words = _text.WordsInQuestions(len).ToList();
        Console.WriteLine(words.Count == 0 ? "Таких слов нет." : string.Join(", ", words));
    }

    private static void RemoveConsonantWords()
    {
        int len = ReadInt("Длина слова: ");
        _text.RemoveWordsStartingWithConsonant(len);
        Console.WriteLine(_text);
    }

    private static void ReplaceWords()
    {
        Console.WriteLine($"Предложений в тексте: {_text.Sentences.Count}");
        int number = ReadInt("Номер предложения (с 1): ");
        int len = ReadInt("Длина заменяемых слов: ");
        Console.Write("Чем заменить: ");
        string replacement = Console.ReadLine() ?? "";

        int count = _text.ReplaceWordsInSentence(number - 1, len, replacement);
        if (count < 0)
            Console.WriteLine("Предложения с таким номером нет.");
        else
        {
            Console.WriteLine($"Замен: {count}");
            Console.WriteLine(_text);
        }
    }

    private static void RemoveStopWords()
    {
        var stop = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (string file in Directory.GetFiles(_dataDir, "stopwords_*.txt"))
        {
            foreach (string line in File.ReadAllLines(file, Encoding.UTF8))
            {
                string w = line.Trim();
                if (w.Length > 0) stop.Add(w);
            }
        }

        _text.RemoveStopWords(stop);
        Console.WriteLine(_text);
    }

    private static void ExportXml()
    {
        string path = Path.Combine(Environment.CurrentDirectory, "output.xml");
        _text.ExportToXml(path);
        Console.WriteLine($"Сохранено: {path}");
    }

    private static void PrintConcordance()
    {
        Dictionary<string, ConcordanceEntry> concordance = _text.BuildConcordance();
        if (concordance.Count == 0)
        {
            Console.WriteLine("Слов нет.");
            return;
        }

        // Алфавитный порядок слов обеспечивает SortedSet ключей.
        var sortedWords = new SortedSet<string>(
            concordance.Keys, StringComparer.Create(CultureInfo.InvariantCulture, false));

        int width = sortedWords.Max(w => w.Length) + 3;

        foreach (string word in sortedWords)
        {
            ConcordanceEntry e = concordance[word];
            Console.WriteLine($"{word.PadRight(width, '.')}{e.Count}: {string.Join(' ', e.Lines)}");
        }
    }

    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int value) && value >= 0)
                return value;
            Console.WriteLine("Введите неотрицательное целое число.");
        }
    }
}
