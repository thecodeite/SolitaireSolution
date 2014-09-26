using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Solitaire.Models;

namespace Tests.UnitTests
{
    [TestFixture]
    class CardTests
    {
        [Test]
        public void back_of_card_should_be_blank()
        {
            var card = new Card();

            var appearance = card.Render();

            appearance.Should().Be("**");
        }

        [Test]
        public void card_should_have_front_and_back_and_start_facing_down_and_can_be_flipped()
        {
            var card = new Card();

            bool isFaceDown = card.IsFaceDown;

            isFaceDown.Should().BeTrue();

            card.Flip();

            isFaceDown = card.IsFaceDown;

            isFaceDown.Should().BeFalse();
        }

        [Test]
        public void front_of_card_should_not_be_blank()
        {
            var card = new Card();
            
            card.Flip();

            card.Render().Should().NotBe("**");
        }

        [Test]
        public void card_has_suit_and_ordinal_defaulting_to_ace_of_diamonds_and_have_ordinal_1()
        {
            var card = new Card();

            var suit = card.Suit;
            var ordinalIndex = card.Ordinal;

            suit.Should().Be("D");
            ordinalIndex.Should().Be(1);
        }

        [Test]
        public void default_card_renders_as_DA()
        {
            var card = new Card();
            card.Flip();

            card.Render().Should().Be("DA");
        }

        [Test]
        public void suit_and_ordnial_are_settable_on_creation()
        {
            var card = new Card("H", 2);

            card.Suit.Should().Be("H");
            card.Ordinal.Should().Be(2);
        }

        [TestCase("D")]
        [TestCase("H")]
        [TestCase("c")]
        [TestCase("s")]
        public void suit_can_be_D_H_c_or_s(string suit)
        {
            new Card(suit, 1);
        }

        [TestCase("d")] // Wrong case
        [TestCase("h")] // Wrong case
        [TestCase("C")] // Wrong case 
        [TestCase("S")] // Wrong case
        [TestCase("X")] // Invalid letter
        [TestCase("7")] // A number
        public void invalid_suits_throw_exception(string suit)
        {
            Action act = () =>
            {
                new Card(suit, 1);
                return;
            };

            act.ShouldThrow<ArgumentException>();
        }

        [TestCase(1, "A")]
        [TestCase(2, "2")]
        [TestCase(3, "3")]
        [TestCase(4, "4")]
        [TestCase(5, "5")]
        [TestCase(6, "6")]
        [TestCase(7, "7")]
        [TestCase(8, "8")]
        [TestCase(9, "9")]
        [TestCase(10, "T")]
        [TestCase(11, "J")]
        [TestCase(12, "Q")]
        [TestCase(13, "K")]
        public void ordinal_can_be_between_1_and_13_inclusive(int ordnial, string expected)
        {
            var card = new Card(Suits.Diamond, ordnial);

            card.Flip();

            card.Render().Should().Be("D" + expected);
        }

        [TestCase("DA", Suits.Diamond, Ordinals.Ace)]
        [TestCase("H6", Suits.Heart, Ordinals.Six)]
        [TestCase("cT", Suits.Club, Ordinals.Ten)]
        [TestCase("sQ", Suits.Spade, Ordinals.Queen)]
        public void can_create_card_from_short_hand(string shortHand, string expectedSuit, int expectedOrdinal)
        {
            Card card = Card.FromShortHand(shortHand);

            card.Suit.Should().Be(expectedSuit);
            card.Ordinal.Should().Be(expectedOrdinal);
        }
    }
}
