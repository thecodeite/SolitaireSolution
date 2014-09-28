using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitaire.Models
{
    public class Card
    {
        private static readonly Dictionary<int, string> RankMap = new Dictionary<int, string>
        {
            {1, "A"},
            {2, "2"},
            {3, "3"},
            {4, "4"},
            {5, "5"},
            {6, "6"},
            {7, "7"},
            {8, "8"},
            {9, "9"},
            {10, "T"},
            {11, "J"},
            {12, "Q"},
            {13, "K"},
        };

        public Card() : this(Suits.Diamond, 1)
        {
        }

        public Card(string suit, int rank, bool isFaceDown = true)
        {
            if(!Suits.AllSuits.Contains(suit))
                throw new ArgumentException("Suit is not valid. Valid values are 'D', 'H', 'c' and 's'. Case is important.", suit);

            if(!RankMap.ContainsKey(rank))
                throw new ArgumentException("Ordnial is not valid. Valid values are >= 1 and <= 13");

            IsFaceDown = isFaceDown;
            Suit = suit;
            Rank = rank;
        }

        /// <summary>
        /// When face down, it is showing the back of the card (i.e. the side with no information on)
        /// </summary>
        public bool IsFaceDown { get; set; }

        public string Suit { get; private set; }

        public int Rank { get; private set; }

        public override string ToString()
        {
            return ToShortHand() + (IsFaceDown ? "v" : "^");
        }

        public string Render()
        {
            return IsFaceDown ? "**" : ToShortHand();
        }

        public string ToShortHand()
        {
            string rank;
            if (!RankMap.TryGetValue(Rank, out rank))
            {
                return Suit + "?";
            }

            return Suit + rank;
        }

        public void Flip()
        {
            IsFaceDown = !IsFaceDown;
        }

        /// <summary>
        /// Parse short hand to a card. The card will be face up.
        /// </summary>
        /// <example>DA would become the Ace of Diamonds</example>
        /// <example>H8 would become the Eight of Hearts</example>
        public static Card FromShortHand(string shortHand)
        {
            Debug.Assert(shortHand.Length == 2);

            var suit = shortHand.Substring(0, 1);
            if(!Suits.AllSuits.Contains(suit))
                throw new Exception("Suit not valid:"+ shortHand);

            var rankRepresentation = shortHand.Substring(1, 1);
            var rank= RankMap.First(x => x.Value == rankRepresentation).Key;


            return new Card(suit, rank);
        }

       
    }
}
