using System.Xml.Serialization;

namespace TextProcessing;

/// <summary>
/// Базовый класс токена. Токен - это либо слово (Word), либо знак препинания (Punctuation).
/// Предложение (Sentence) хранит список токенов.
/// </summary>
public abstract class Token
{
    public string Value { get; set; } = "";

    public override string ToString() => Value;
}

/// <summary>Слово: неразрывная последовательность букв/цифр (может содержать ' или - внутри).</summary>
public class Word : Token
{
    /// <summary>Номер строки исходного файла (с 1). Нужен для конкорданса (Project04).</summary>
    public int Line { get; set; }

    [XmlIgnore]
    public int Length => Value.Length;

    // Конструктор без параметров обязателен для XmlSerializer.
    public Word() { }

    public Word(string value, int line)
    {
        Value = value;
        Line = line;
    }
}

/// <summary>Знак препинания: один символ (. , ! ? ; : - и т.д.).</summary>
public class Punctuation : Token
{
    [XmlIgnore]
    public bool IsSentenceEnd => Value is "." or "!" or "?" or "…";

    public Punctuation() { }

    public Punctuation(string value)
    {
        Value = value;
    }
}
