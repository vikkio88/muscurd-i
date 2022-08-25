using System;
using System.Linq;
using System.Collections.Generic;
namespace Muscurdi.Models;
public class MasterPassword
{
    private const int MEMORABLE_SIZE = 5;
    private const int WORD_SIZE = 4;
    private (string, string, string) PrefixWords { get; init; }
    private string FinalWord { get; init; } = string.Empty;
    private int NumericAppendix { get; init; }

    public static MasterPassword Make(List<string> prefix, string final, int numericAppendix)
    {
        if (prefix.Count != (MEMORABLE_SIZE - 2))
        {
            throw new ArgumentException($"Not enough prefix words: expected {MEMORABLE_SIZE - 2} got {prefix.Count}");
        }

        if (numericAppendix < 1000 || numericAppendix > 9999)
        {
            throw new ArgumentException($"Wrong numeric appendix {numericAppendix}");
        }

        var totalLength = prefix.Sum(w => w.Length) + final.Length + $"{numericAppendix}".Length;
        if (totalLength != MEMORABLE_SIZE * WORD_SIZE)
        {
            throw new ArgumentException($"Wrong size of words: {totalLength}, instead of {MEMORABLE_SIZE * WORD_SIZE}");
        }

        return new()
        {
            PrefixWords = (prefix[0], prefix[1], prefix[2]),
            FinalWord = final,
            NumericAppendix = numericAppendix
        };
    }
    public static MasterPassword Make(string memorable)
    {
        var split = memorable.Split('-');
        if (split.Count() != MEMORABLE_SIZE)
        {
            throw new ArgumentException($"Wrong size of memorable: {split.Count()}, instead of {MEMORABLE_SIZE}");
        }

        int numberAppendix;
        var parseResult = int.TryParse(split[4], out numberAppendix);

        if (!parseResult || (numberAppendix < 1000 || numberAppendix > 9999))
        {
            throw new ArgumentException($"Invalid numeric appendix");
        }

        return Make(split.Take(3).ToList(), split.TakeLast(2).First(), numberAppendix);
    }

    public string ToMemorable()
    {
        var (one, two, three) = PrefixWords;
        return $"{one}-{two}-{three}-{FinalWord}-{NumericAppendix}";
    }

    public string ToKey()
    {
        return $"{this}".Replace("-", "");
    }

    public override string ToString()
    {
        var (one, two, three) = PrefixWords;
        return $"{string.Join("-", Enumerable.Repeat($"{one}-{two}-{three}", 2))}-{FinalWord}-{NumericAppendix}";
    }
}
