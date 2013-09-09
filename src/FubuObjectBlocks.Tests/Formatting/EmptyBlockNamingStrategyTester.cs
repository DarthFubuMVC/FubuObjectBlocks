﻿using FubuObjectBlocks.Formatting;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuObjectBlocks.Tests.Formatting
{
    [TestFixture]
    public class EmptyBlockNamingStrategyTester
    {
        private EmptyBlockNamingStrategy theStrategy { get { return new EmptyBlockNamingStrategy(); } }

        [Test]
        public void matches_empty()
        {
            theStrategy.Matches(BlockName.Empty).ShouldBeTrue();
        }

        [Test]
        public void formats_as_null()
        {
            theStrategy.NameFor(BlockName.Empty).ShouldBeNull();
        }
    }
}