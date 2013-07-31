using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InlineScheduler.Advanced;
using NUnit.Framework;
using FluentAssertions;
using InlineScheduler.Advanced.State;

namespace InlineScheduler.Tests.Advanced
{
    public class When_work_item_has_not_run_since_before_interval
    {
        JobItem _item;

        [TestFixtureSetUp]
        public void Given_work_item() {

            var state = TempFolderStateProvider.CreateInTempFolder("When_work_item_has_not_run_since_before_interval" + Guid.NewGuid().ToString());
            
            state.Store("Foo1", new WorkState {
                LastCompleteTime = new DateTime(11, 10, 09)
            });

            var ctx = new TestSchedulerContext();
            ctx.State = state;

            // Make workitem with LastComplete from 9-10-2011
            _item = WorkItemFactory.Create(ctx);

            // Force workitem to reschedule            
            _item.UpdateState();
        }

        [Test]
        public void Should_be_scheduled() {
            _item.Status.Should().Be(JobStatus.Scheduled);
        }
    }
}
