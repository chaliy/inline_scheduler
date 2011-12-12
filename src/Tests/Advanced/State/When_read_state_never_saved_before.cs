using NUnit.Framework;
using InlineScheduler.Advanced.State;
using System;
using System.IO;
using FluentAssertions;

namespace InlineScheduler.Tests.Advanced.State
{
    public class When_read_state_never_saved_before
    {
        private string _storagePath;
        private WorkState _readState;

        [TestFixtureSetUp]
        public void Given_() 
        {
            var state = TempFolderStateProvider.CreateInTempFolder("When_read_state_never_saved_before_" + Guid.NewGuid().ToString());
            _storagePath = state.StoragePath;
            _readState = state.Retrieve("Foo1");
        }

        [Test]
        public void Should_store_state() 
        {
            _readState.Should().BeNull();
        }
    }
}
