using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Solitaire.Models;
using Tests.Utils;

namespace Tests.UnitTests
{
    [TestFixture]
    class GameTests
    {
        [Test]
        public void can_quit_the_game_with_q()
        {
            var game = new Game();

            game.Quit.Should().BeFalse();

            game.ParseInput("Q");

            game.Quit.Should().BeTrue();
        }

        [Test]
        public void deal_will_fill_columns_and_stack_and_leave_discard_empty()
        {
            var game = new Game();

            game.Deal(Deck.MakeDefaultDeck());

            game.Columns.Count.Should().Be(7);

            // Check the columns contain the right cards. The ^ = face up, v = face down.
            game.Columns[1].ToStringList().Should().Be("DA^");
            game.Columns[2].ToStringList().Should().Be("D2v D8^");
            game.Columns[3].ToStringList().Should().Be("D3v D9v HA^");
            game.Columns[4].ToStringList().Should().Be("D4v DTv H2v H6^");
            game.Columns[5].ToStringList().Should().Be("D5v DJv H3v H7v HT^");
            game.Columns[6].ToStringList().Should().Be("D6v DQv H4v H8v HJv HK^");
            game.Columns[7].ToStringList().Should().Be("D7v DKv H5v H9v HQv cAv c2^");

            // Check the stack contains the rest of the cards face up and in reverse
            game.Stack.ToStringList()
                .Should()
                .Be("sK^ sQ^ sJ^ sT^ s9^ s8^ s7^ s6^ s5^ s4^ s3^ s2^ sA^ cK^ cQ^ cJ^ cT^ c9^ c8^ c7^ c6^ c5^ c4^ c3^");

            game.DiscardPiles.Count.Should().Be(4);
            game.DiscardPiles[Suits.Diamond].Should().BeEmpty();
            game.DiscardPiles[Suits.Heart].Should().BeEmpty();
            game.DiscardPiles[Suits.Club].Should().BeEmpty();
            game.DiscardPiles[Suits.Spade].Should().BeEmpty();
        }
    }
}
