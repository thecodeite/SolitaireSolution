using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitaire.Models
{
    public class Card
    {
        private static readonly Dictionary<int, string> OrdinalMap = new Dictionary<int, string>
        {
            {1, "A"}
        };

        public Card() : this(Suits.Diamond, 1)
        {
        }

        public Card(string suit, int ordinal)
        {
            if(!Suits.AllSuits.Contains(suit))
                throw new ArgumentException("Suit is not valid. Valid values are D H c s", suit);

            IsFaceDown = true;
            Suit = suit;
            Ordinal = ordinal;
        }

        public bool IsFaceDown { get; private set; }

        public string Suit { get; private set; }

        public int Ordinal { get; private set; }

        public string Render()
        {
            if (IsFaceDown)
            {
                return "**";
            }

            string ordinal;
            if (!OrdinalMap.TryGetValue(Ordinal, out ordinal))
            {
                return Suit + "?";
            }

            return Suit + ordinal;
        }

        public void Flip()
        {
            IsFaceDown = !IsFaceDown;
        }
    }
}
