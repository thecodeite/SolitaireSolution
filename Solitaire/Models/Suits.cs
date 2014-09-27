using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Solitaire.Models
{
    public static class Suits
    {
       // D - Diamond
        public const string Diamond = "D";

        //H - Heart
        public const string Heart = "H";

        //c - Club (lowercase)
        public const string Club = "c";

        //s - Spade (lowercase)
        public const string Spade = "s";

        public static string[] AllSuits =
        {
            Diamond, Heart, Club, Spade
        };

        public static IEnumerable<string> AllSuitsUpperCase
        {
            get { return AllSuits.Select(x => x.ToUpperInvariant()); }
        }
    }
}
