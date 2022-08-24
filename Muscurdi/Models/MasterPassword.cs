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
        var splitCount = split.Select(w => w.Length).Sum();
        if (splitCount != MEMORABLE_SIZE * WORD_SIZE)
        {
            throw new ArgumentException($"Wrong size of words: {splitCount}, instead of {MEMORABLE_SIZE * WORD_SIZE}");
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

    public override string ToString()
    {
        var (one, two, three) = PrefixWords;
        return $"{string.Join("-", Enumerable.Repeat($"{one}-{two}-{three}", 2))}-{FinalWord}-{NumericAppendix}";
    }
}
