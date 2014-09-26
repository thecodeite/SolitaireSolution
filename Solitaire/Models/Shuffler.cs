using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitaire.Models
{
    public static class Shuffler
    {
        // Taken with no shame from first answer in SO and modified slightly. 422 upvodes means = good, right?
        // http://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp
        // (Also, missing ruby idiom of using '!' in name to signify this is in-place shuffling.)
        public static void Shuffle<T>(this IList<T> list, Random inputRandom = null)
        {
            Random rng = inputRandom ?? new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }
}
