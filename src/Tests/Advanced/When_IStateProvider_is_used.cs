using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading;
using InlineScheduler.Advanced.State;
using FluentAssertions;

namespace InlineScheduler.Tests.Advanced
{
    public class When_IStateProvider_is_used
    {
        IStateProvider state;
        TestSchedulerContext ctx;

        [TestFixtureSetUp]
        public void Given_() {
            ctx = new TestSchedulerContext();

            state = ctx.State;

            var _item = WorkItemFactory.Create(ctx);

            // Make item think that now tomorrow, so 
            // it became applicable for scheduling            
            ctx.MoveToTommorrow();
            _item.UpdateState();
            _item.Run();

            // and .. wait until it will complete
            Wait.Unitl(() => _item.Status == JobStatus.Pending);
        }

        [Test]
        public void State_should_be_persisted() {

            var workstate = state.Retrieve("Foo1");

            workstate.Should().NotBeNull();

            workstate.LastCompleteTime.HasValue.Should().BeTrue();

            Assert.AreEqual(workstate.LastCompleteTime, ctx.GetCurrentTime());
        }
    }
}
