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
    [UseReporter(typeof(DiffReporter))]
    //[UseReporter(typeof(VisualStudioReporter))]
    class GameStartupTest
    {
        [Test]
        public void the_board_should_render_empty_before_we_deal()
        {
            // When the game is started
            var game = new Game();

            // the board should be correct
            string board = game.Render();
            Approvals.Verify(board);
        }

        [Test]
        public void the_board_should_appear_correct_on_startup()
        {
            // When the game is started
            var game = new Game();

            game.Deal(Deck.MakeTestDeck());

            // the board should be correct
            string board = game.Render();
            Approvals.Verify(board);
        }

        [Test]
        public void the_board_should_appear_correct_if_we_cheat()
        {
            // When the game is started
            var game = new Game();

            game.Cheat();

            // the board should be correct
            string board = game.Render();
            Approvals.Verify(board);
        }
    }
}
