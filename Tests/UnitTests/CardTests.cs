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
    }
}
