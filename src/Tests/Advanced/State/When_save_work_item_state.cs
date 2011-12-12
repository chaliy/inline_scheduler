using NUnit.Framework;
using InlineScheduler.Advanced.State;
using System;
using System.IO;
using FluentAssertions;

namespace InlineScheduler.Tests.Advanced.State
{
    public class When_save_work_item_state
    {
        private string _storagePath;

        [TestFixtureSetUp]
        public void Given_() 
        {
            var state = TempFolderStateProvider.CreateInTempFolder("When_save_work_item_state_" + Guid.NewGuid().ToString());
            _storagePath = state.StoragePath;
            state.Store("Foo1", new WorkState());
        }

        [Test]
        public void Should_store_state() 
        {
            File.Exists(Path.Combine(_storagePath, "Foo1")).Should().BeTrue();
        }

    }
}
