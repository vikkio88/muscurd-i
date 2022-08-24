using System.Linq;
using System;
namespace Muscurdi.Models;
public class MasterPassword
{
    private const int MEMORABLE_SIZE = 5;
    private const int WORD_SIZE = 4;
    private (string, string, string) PrefixWords { get; init; }
    private string FinalWord { get; init; } = string.Empty;
    private int NumericAppendix { get; init; }

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

        int numberAppedinx;
        var parseResult = int.TryParse(split[4], out numberAppedinx);

        if (!parseResult || (numberAppedinx < 1000 || numberAppedinx > 9999))
        {
            throw new ArgumentException($"Invalid numeric appendix");
        }

        return new()
        {
            PrefixWords = (split[0], split[1], split[2]),
            FinalWord = split[3],
            NumericAppendix = numberAppedinx
        };
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
