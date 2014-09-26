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
    }
}
