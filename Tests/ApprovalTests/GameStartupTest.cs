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
    class GameStartupTest
    {
        [Test]
        public void the_board_should_appear_correct_on_startup()
        {
            // When the game is started
            var game = new Game();

            // the board should be correct
            string board = game.Render();
            Approvals.Verify(board);
        }
    }
}
