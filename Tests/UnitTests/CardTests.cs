using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests.UnitTests
{
    [TestFixture]
    class CardTests
    {
        [Test]
        public void back_of_card_should_be_blank()
        {
            dynamic card = new object();

            var appearance = card.render();

            appearance.Should.Be("**");
        }
    }
}
