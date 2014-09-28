using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Solitaire.Helpers;
using Solitaire.Models;
using Tests.Utils;

namespace Tests.UnitTests
{
    [TestFixture]
    internal class GameTests
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

            // Check the columns contain the right cards. The ^ = face up, v = face down.
            game.Locations["1"].ToStringList().Should().Be("DA^");
            game.Locations["2"].ToStringList().Should().Be("D2v D8^");
            game.Locations["3"].ToStringList().Should().Be("D3v D9v HA^");
            game.Locations["4"].ToStringList().Should().Be("D4v DTv H2v H6^");
            game.Locations["5"].ToStringList().Should().Be("D5v DJv H3v H7v HT^");
            game.Locations["6"].ToStringList().Should().Be("D6v DQv H4v H8v HJv HK^");
            game.Locations["7"].ToStringList().Should().Be("D7v DKv H5v H9v HQv cAv c2^");

            // Check the stack contains the rest of the cards face up and in reverse
            game.Stack.ToStringList()
                .Should()
                .Be("sK^ sQ^ sJ^ sT^ s9^ s8^ s7^ s6^ s5^ s4^ s3^ s2^ sA^ cK^ cQ^ cJ^ cT^ c9^ c8^ c7^ c6^ c5^ c4^ c3^");

            game.Locations[Suits.Diamond].Should().BeEmpty();
            game.Locations[Suits.Heart].Should().BeEmpty();
            game.Locations[Suits.Club.ToUpperInvariant()].Should().BeEmpty();
            game.Locations[Suits.Spade.ToUpperInvariant()].Should().BeEmpty();
        }

        [Test]
        public void t_should_move_three_cards_from_stack_to_waste()
        {
            var game = new Game();
            game.Deal(Deck.MakeDefaultDeck());

            game.Stack.ToStringList()
                .Should()
                .Be("sK^ sQ^ sJ^ sT^ s9^ s8^ s7^ s6^ s5^ s4^ s3^ s2^ sA^ cK^ cQ^ cJ^ cT^ c9^ c8^ c7^ c6^ c5^ c4^ c3^");
            game.WastePile.Should().BeEmpty();

            game.ParseInput("T");

            game.Stack.ToStringList()
                .Should()
                .Be("sT^ s9^ s8^ s7^ s6^ s5^ s4^ s3^ s2^ sA^ cK^ cQ^ cJ^ cT^ c9^ c8^ c7^ c6^ c5^ c4^ c3^");

            game.WastePile.ToStringList()
                .Should()
                .Be("sK^ sQ^ sJ^");

        }

        [Test]
        public void t_should_refresh_stack_from_waste_if_stack_three_or_less()
        {
            var game = new Game();
            game.Deal(Deck.MakeDefaultDeck());

            // Move 3 off the stack 7 times
            7.Times(x => game.ParseInput("T"));

            game.Stack.ToStringList()
                .Should()
                .Be("c5^ c4^ c3^");
            game.WastePile.ToStringList()
                .Should()
                .Be("sK^ sQ^ sJ^ sT^ s9^ s8^ s7^ s6^ s5^ s4^ s3^ s2^ sA^ cK^ cQ^ cJ^ cT^ c9^ c8^ c7^ c6^");

            game.ParseInput("T");

            game.Stack.ToStringList()
                .Should()
                .Be("sK^ sQ^ sJ^ sT^ s9^ s8^ s7^ s6^ s5^ s4^ s3^ s2^ sA^ cK^ cQ^ cJ^ cT^ c9^ c8^ c7^ c6^ c5^ c4^ c3^");

            game.WastePile.ToStringList()
                .Should().BeEmpty();

        }

        [Test]
        public void can_move_card_from_stack_to_column()
        {
            var game = new Game();
            game.Deal(Deck.MakeDefaultDeck());

            game.Stack.ToStringList()
                .Should()
                .Be("sK^ sQ^ sJ^ sT^ s9^ s8^ s7^ s6^ s5^ s4^ s3^ s2^ sA^ cK^ cQ^ cJ^ cT^ c9^ c8^ c7^ c6^ c5^ c4^ c3^");
            game.Locations["1"].ToStringList().Should().Be("DA^");

            game.ParseInput("SK 1");

            game.Message.Should().Be("Moving SK to 1");
            game.Stack.ToStringList()
                .Should()
                .Be("sQ^ sJ^ sT^ s9^ s8^ s7^ s6^ s5^ s4^ s3^ s2^ sA^ cK^ cQ^ cJ^ cT^ c9^ c8^ c7^ c6^ c5^ c4^ c3^");
            game.Locations["1"].ToStringList().Should().Be("DA^ sK^");

        }

        [Test]
        [Ignore("Not implimented correctly")]
        public void can_move_card_from_column_to_discard()
        {
            var game = new Game();
            game.Deal(Deck.MakeDefaultDeck());

            game.Stack.ToStringList()
                .Should()
                .Be("sK^ sQ^ sJ^ sT^ s9^ s8^ s7^ s6^ s5^ s4^ s3^ s2^ sA^ cK^ cQ^ cJ^ cT^ c9^ c8^ c7^ c6^ c5^ c4^ c3^");
            game.Locations["1"].ToStringList().Should().Be("DA^");

            game.ParseInput("SK 1");

            game.Message.Should().Be("Moving SK to 1");
            game.Stack.ToStringList()
                .Should()
                .Be("sQ^ sJ^ sT^ s9^ s8^ s7^ s6^ s5^ s4^ s3^ s2^ sA^ cK^ cQ^ cJ^ cT^ c9^ c8^ c7^ c6^ c5^ c4^ c3^");
            game.Locations["1"].ToStringList().Should().Be("DA^ sK^");

        }

        [Test]
        public void moving_cards_moves_all_stacked_cards()
        {
            var game = new Game();
            game.Deal(Deck.MakeTestDeck());

            game.Locations["2"].ToStringList().Should().Be("DAv sK^");
            game.Locations["6"].ToStringList().Should().Be("HTv HQv H4v H8v HJv cJ^");
            game.Locations["7"].ToStringList().Should().Be("D7v DKv H5v H9v cQv cAv DQ^");

            game.ParseInput("cJ 7");

            game.Locations["2"].ToStringList().Should().Be("DAv sK^");
            game.Locations["6"].ToStringList().Should().Be("HTv HQv H4v H8v HJ^");
            game.Locations["7"].ToStringList().Should().Be("D7v DKv H5v H9v cQv cAv DQ^ cJ^");

            game.ParseInput("DQ 2");

            game.Locations["2"].ToStringList().Should().Be("DAv sK^ DQ^ cJ^");
            game.Locations["6"].ToStringList().Should().Be("HTv HQv H4v H8v HJ^");
            game.Locations["7"].ToStringList().Should().Be("D7v DKv H5v H9v cQv cA^");
        }

    }
}
