using NUnit.Framework;
using InlineScheduler.Advanced;
using System.Threading.Tasks;
using System;
using FluentAssertions;

namespace InlineScheduler.Tests.Advanced
{    
    public class When_create_work_item
    {
        WorkItem _item;

        [TestFixtureSetUp]
        public void Given_work_item() 
        {
            _item = new WorkItem(new DefaultSchedulerContext(), "Foo1", () =>
            {
                return Task.Factory.StartNew(() => Console.WriteLine("Foo1"));
            }, TimeSpan.FromMinutes(10), null);
        }

        [Test]
        public void Should_be_pending() 
        {
            _item.Status.Should().Be(InlineScheduler.Advanced.WorkStatus.Pending);                       
        }
    }
}
