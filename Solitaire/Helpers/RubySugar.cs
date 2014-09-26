using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitaire.Helpers
{
    public static class RubySugar
    {
        public static void Times(this int repeatCount, Action<int> action)
        {
            for (var i = 0; i < repeatCount; i++)
            {
                action(i);
            }
        }
    }
}
