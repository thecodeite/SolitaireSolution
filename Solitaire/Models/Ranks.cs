using System.Linq;

namespace Solitaire.Models
{
    public static class Ranks
    {
        public const int Ace = 1;
        public const int Two = 2;
        public const int Three = 3;
        public const int Four = 4;
        public const int Five = 5;
        public const int Six = 6;
        public const int Seven = 7;
        public const int Eight = 8;
        public const int Nine = 9;
        public const int Ten = 10;
        public const int Jack = 11;
        public const int Queen = 12;
        public const int King = 13;

        public static readonly int[] AllRanks = Enumerable.Range(1, 13).ToArray();
    }
}