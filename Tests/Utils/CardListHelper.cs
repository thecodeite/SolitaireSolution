using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solitaire.Models;

namespace Tests.Utils
{
    static class CardListHelper
    {
        public static string ToStringList(this IEnumerable<Card> list)
        {
            return string.Join(" ", list);
        }
    }
}
