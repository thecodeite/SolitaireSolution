using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitaire.Models
{
    public class Deck
    {
        public const string TestDeck = "HA DA D3 D4 D5 HT D7 sK D9 DT DJ cQ DK " +
                                       "D8 H2 H3 H4 H5 c3 H7 H8 H9 D6 HJ HQ cJ " +
                                       "cA DQ H6 c4 c5 c6 c7 c8 c9 cT HK c2 cK " +
                                       "sA s2 s3 s4 s5 s6 s7 s8 s9 sT sJ sQ D2";

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

        public static Deck MakeTestDeck()
        {
            var cards = TestDeck.Split(' ').Select(Card.FromShortHand);
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

        public Card TakeCard()
        {
            return Cards.Dequeue();
        }

        public void Flip()
        {
            Cards = new Queue<Card>(Cards.Reverse());
            foreach (var card in Cards)
            {
                card.Flip();
            }

        }
    }
}
