﻿using System;
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
    class CardTests
    {
        [Test]
        public void back_of_card_should_be_blank()
        {
            var card = new Card();

            var appearance = card.Render();

            appearance.Should().Be("**");
        }

        [Test]
        public void card_should_have_front_and_back_and_start_facing_down()
        {
            dynamic card = new Card();

            bool isFaceDown = card.IsFaceDown;

            isFaceDown.Should().BeTrue();
        }
    }
}