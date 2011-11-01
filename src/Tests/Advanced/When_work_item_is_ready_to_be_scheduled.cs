using NUnit.Framework;
using InlineScheduler.Advanced;
using FluentAssertions;
using System;
using System.Diagnostics;

namespace InlineScheduler.Tests.Advanced
{
    public class When_work_item_is_ready_to_be_scheduled
    {
        WorkItem _item;

        [TestFixtureSetUp]
        public void Given_work_item() 
        {
            var ctx = new TestWorkContext();
            // Make workitem think that it was created yesterday
            ctx.MoveToYesterday();            
            _item = WorkItemFactory.Create(ctx);

            // Now update date to today, this 
            // will make workitem applicable for scheduling
            ctx.MoveToNow(); 

            // Force workitem to reschedule            
            _item.UpdateState();
        }

        [Test]
        public void Should_be_scheduled() 
        {
            _item.Status.Should().Be(WorkStatus.Scheduled);                       
        }
    }
}
