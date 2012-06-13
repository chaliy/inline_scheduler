﻿using NUnit.Framework;
using InlineScheduler.Advanced;
using FluentAssertions;
using System.Threading;

namespace InlineScheduler.Tests.Advanced
{
    public class When_work_item_is_run
    {
        JobItem _item;

        [TestFixtureSetUp]
        public void Given_work_item() 
        {
            var ctx = new TestSchedulerContext();            
            _item = WorkItemFactory.Create(ctx);            

            // Make item think that now tomorrow, so 
            // it became applicable for scheduling            
            ctx.MoveToTommorrow();
            _item.UpdateState();
            _item.Run();

            // and .. wait until it will complete
            while (true) 
            {
                if (_item.Status == JobStatus.Pending) 
                {
                    break;
                }
                Thread.Sleep(200);
            }
        }

        [Test]
        public void Should_be_spending() 
        {
            _item.Status.Should().Be(JobStatus.Pending);                       
        }

        [Test]
        public void Should_fill_previous_runs()
        {
            _item.PreviousRuns.Should().NotBeEmpty();
        }        
    }
}
