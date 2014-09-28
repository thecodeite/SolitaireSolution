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
    class PileTests
    {
        [Test]
        public void can_get_column_of_a_pile()
        {
            var pile = new Pile('4');

            pile.IsColumn.Should().BeTrue();
            pile.Column.Should().Be(4);
        }
    }
}
