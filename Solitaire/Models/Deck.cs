using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitaire.Models
{
    public class Deck
    {
        public Deck(IEnumerable<Card> cards)
        {
            Cards = new Queue<Card>(cards);
        }

        public static Deck MakeDefaultDeck()
        {
            return new Deck(MakeAllCards());
        }

        public static Deck MakeShuffledDeck(Random random = null)
        {
            var cards = MakeAllCards().ToList();
            cards.Shuffle(random);
            return new Deck(cards);
        }

        private static IEnumerable<Card> MakeAllCards()
        {
            foreach (var suit in Suits.AllSuits)
            {
                foreach (var ordinal in Ordinals.AllOrdinals)
                {
                    yield return new Card(suit, ordinal);
                }
            }
        }

        public Queue<Card> Cards { get; private set; }
    }
}
