using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;
using Solitaire.Models;

namespace Tests.ApprovalTests
{

    [TestFixture]
    [UseReporter(typeof (DiffReporter))]
    //[UseReporter(typeof(VisualStudioReporter))]
    internal class MovementTests
    {
        [Test]
        public void can_make_legal_move_from_one_column_to_another()
        {
            var game = new Game();
            game.Deal(Deck.MakeTestDeck());

            // Move jack of clubs from column 6 to column 7
            game.ParseInput("cJ 7");

            string board = game.Render();
            Approvals.Verify(board);
        }
        
        [Test]
        public void can_make_legal_move_from_one_column_to_another_and_child_cards_move_too()
        {
            var game = new Game();
            game.Deal(Deck.MakeTestDeck());

            // Move jack of clubs from column 6 to column 7
            game.ParseInput("cJ 7");

            // Move queene of diamonds from column 7 to column 2
            game.ParseInput("DQ 2");

            string board = game.Render();
            Approvals.Verify(board);
        }

        [Test]
        public void can_make_legal_move_ace_from_column_to_waste()
        {
            var game = new Game();
            game.Deal(Deck.MakeTestDeck());

            // Move ace of hearts to hearts discard pile
            game.ParseInput("HA H");

            string board = game.Render();
            Approvals.Verify(board);
        }

        [Test]
        public void can_make_legal_move_king_to_empty_space()
        {
            var game = new Game();
            game.Deal(Deck.MakeTestDeck());

            // Move ace of hearts to hearts discard pile
            game.ParseInput("HA H");

            // Move king of X to column 1
            game.ParseInput("sK 1");

            string board = game.Render();
            Approvals.Verify(board);
        }

        [Test]
        public void can_make_legal_move_card_from_stack_to_columns()
        {
            var game = new Game();
            game.Deal(Deck.MakeTestDeck());

            // Move two of diamonds from stack to column 4
            game.ParseInput("D2 4");

            string board = game.Render();
            Approvals.Verify(board);
        }
    }
}
