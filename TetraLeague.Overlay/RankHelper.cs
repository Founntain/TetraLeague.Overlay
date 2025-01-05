namespace TetraLeague.Overlay;

public class RankHelper
{
    public static int GetRankAsNumber(string rank)
    {
        return rank switch
        {
            "d" => 0,
            "d+" => 1,
            "c-" => 2,
            "c" => 3,
            "c+" => 4,
            "b-" => 5,
            "b" => 6,
            "b+" => 7,
            "a-" => 8,
            "a" => 9,
            "a+" => 10,
            "s-" => 11,
            "s" => 12,
            "s+" => 13,
            "ss" => 14,
            "u" => 15,
            "x" => 16,
            "x+" => 17,
            _ => -1
        };
    }
}