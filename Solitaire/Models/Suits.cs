using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitaire.Models
{
    public static class Suits
    {
       // D - Diamond
        public static string Diamond = "D";

        //H - Heart
        public static string Heart = "H";

        //c - Club (lowercase)
        public static string Club = "c";

        //s - Spade (lowercase)
        public static string Spade = "s";

        public static string[] AllSuits =
        {
            Diamond, Heart, Club, Spade
        };
    }
}
