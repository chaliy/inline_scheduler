using NUnit.Framework;
using InlineScheduler.Advanced.State;
using System;
using System.IO;
using FluentAssertions;

namespace InlineScheduler.Tests.Advanced.State
{
    public class When_save_and_read_work_item_state
    {
        private WorkState _readState;

        [TestFixtureSetUp]
        public void Given_() 
        {
            var state = TempFolderStateProvider.CreateInTempFolder("When_save_work_item_state_" + Guid.NewGuid().ToString());            
            state.Store("Foo1", new WorkState
            {
                LastCompleteTime = new DateTime(11, 10, 09)
            });
            _readState = state.Retrieve("Foo1");
        }

        [Test]
        public void Should_read_something() 
        {
            _readState.Should().NotBeNull();
        }

        [Test]
        public void Should_read_correct_last_complete_time()
        {
            _readState.LastCompleteTime.Should().Be(new DateTime(11, 10, 09));
        }
    }
}
