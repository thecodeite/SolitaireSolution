using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;
using Solitaire.Models;

namespace Tests.ApprovalTests
{
    [TestFixture]
    [UseReporter(typeof (DiffReporter))]
    //[UseReporter(typeof(VisualStudioReporter))]
    internal class IllegalMovementTests
    {
        [Test]
        public void can_make_illegal_move_of_face_down_card()
        {
            var game = new Game();
            game.Deal(Deck.MakeTestDeck());

            // Move face down queen of hearts to column 2 on top of king of spades
            game.ParseInput("HQ 2");

            string board = game.Render();
            Approvals.Verify(board);
        }
        
        [Test]
        [Ignore]
        // Not sure of the value of this test as game has to be in illegal state for this rule to occur
        public void cards_may_be_moved_even_if_cards_below_are_not_sequential_descending_and_of_alternating_case()
        {
            var game = new Game();
            game.Deal(Deck.MakeTestDeck());

            // Move two of diamonds on top of queen of hearts in column 7
            // This move should probably not be legal in the end either, but is
            game.ParseInput("D2 7");

            // Move queene of diamonds from column 7 to column 2
            game.ParseInput("DQ 2");

            string board = game.Render();
            Approvals.Verify(board);
        }

        [Test]
        [Ignore]
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
        [Ignore]
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
        [Ignore]
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