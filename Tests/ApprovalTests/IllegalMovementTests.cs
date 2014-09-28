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
        // Cards may only be moved to a numbered column if the lowest card on it is of the same suit and is one higher
        // Postponing writing test due to dubious specification.
        public void can_make_illegal_move_of_card_on_top_of_same_suit()
        {
            var game = new Game();
            game.Deal(Deck.MakeTestDeck());

            // Move ace of hearts to hearts discard pile
            //game.ParseInput("HA H");

            string board = game.Render();
            //Approvals.Verify(board);
        }

        [Test]
        // Kings may be moved to columns that are empty
        public void can_make_illigal_move_of_card_other_than_king_onto_empty_space()
        {
            var game = new Game();
            game.Deal(Deck.MakeTestDeck());

            // Move ace of hearts to hearts discard pile to make a space
            game.ParseInput("HA H");

            // Move eight of diamonds to column 1
            game.ParseInput("D8 1");

            string board = game.Render();
            Approvals.Verify(board);
        }

        [Test]
        // Aces may be moved to the D,H,C,S discard piles if they are empty
        public void can_make_illigal_move_of_card_other_than_ace_onto_empty_discard_pile()
        {
            var game = new Game();
            game.Deal(Deck.MakeTestDeck());

            // Move eight of diamonds from stack to hearts discard pile
            game.ParseInput("D8 D");

            string board = game.Render();
            Approvals.Verify(board);
        }

        [Test]
        // Aces may be moved to the D,H,C,S discard piles if they are empty
        public void can_make_illigal_move_of_card_of_wrong_suit_to_discard_pile()
        {
            var game = new Game();
            game.Deal(Deck.MakeTestDeck());

            // Move ace of hearts from stack to diamonds discard pile
            game.ParseInput("HA D");

            string board = game.Render();
            Approvals.Verify(board);
        }

        [Test]
        // Cards may only be moved to a D,H,C,S dicard pile if the lowest card on it is of the same suit and is one lower
        public void can_make_illegal_move_of_non_sequential_card_to_discard_pile()
        {
            var game = new Game();
            game.Deal(Deck.MakeTestDeck());

            // Move ace of hearts from stack to hearts discard pile
            game.ParseInput("HA H");

            // Move the jack out of the way
            game.ParseInput("cJ 7");

            game.ParseInput("HJ H");

            string board = game.Render();
            Approvals.Verify(board);
        }
    }
}