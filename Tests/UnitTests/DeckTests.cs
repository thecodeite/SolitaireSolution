using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Solitaire.Models;

namespace Tests.UnitTests
{
    [TestFixture]
    class DeckTests
    {
        [Test]
        public void default_deck_contains_all_cards()
        {
            var deck = Deck.MakeDefaultDeck();

            var deckCards = deck.Cards.Select(x => x.ToShortHand());

            EnumerableToString(deckCards).Should().Be(
                "DA D2 D3 D4 D5 D6 D7 D8 D9 DT DJ DQ DK " +
                "HA H2 H3 H4 H5 H6 H7 H8 H9 HT HJ HQ HK " +
                "cA c2 c3 c4 c5 c6 c7 c8 c9 cT cJ cQ cK " +
                "sA s2 s3 s4 s5 s6 s7 s8 s9 sT sJ sQ sK");
        }

        public void shuffled_deck_contains_all_cards_but_in_different_order()
        {
            var deck = Deck.MakeShuffledDeck();


            var deckCards = deck.Cards.Select(x => x.ToShortHand()).ToList();

            deckCards.Count().Should().Be(52);
            EnumerableToString(deckCards).Should().NotBe(
                "DA D2 D3 D4 D5 D6 D7 D8 D9 DT DJ DQ DK " +
                "HA H2 H3 H4 H5 H6 H7 H8 H9 HT HJ HQ HK " +
                "cA c2 c3 c4 c5 c6 c7 c8 c9 cT cJ cQ cK " +
                "sA s2 s3 s4 s5 s6 s7 s8 s9 sT sJ sQ sK");
        }

        [Test]
        [Ignore("No need to run in a full test.")]
        public void array_to_string_method_works()
        {
            var array = new[] {1, 2, 3, 4, 5};

            EnumerableToString(array).Should().Be("1 2 3 4 5");
        }

        [Test]
        public void shuffel_probably_works()
        {
            var array = new[] { 1, 2, 3, 4, 5 };

            Shuffler.Shuffle(array, new Random(2));

            // Assert depends on implimentation of Random not changing as in theory a 
            // random sequence could mess up and return the array exactly as it started
            EnumerableToString(array).Should().NotBe("1 2 3 4 5");
            Console.WriteLine(EnumerableToString(array));

            // Less value than previouse assert
            EnumerableToString(array).Should().NotBe("3 5 1 2 4");
        }

        private string EnumerableToString<T>(IEnumerable<T> sequence)
        {
            var result = new StringBuilder();
            foreach (var item in sequence)
            {
                result.Append(item);
                result.Append(" ");
            }

            // Trim off extra space
            if (result.Length > 0)
                result.Length = result.Length - 1;

            return result.ToString();
        }
    }
}
