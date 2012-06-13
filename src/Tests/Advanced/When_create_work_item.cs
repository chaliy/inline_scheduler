using NUnit.Framework;
using InlineScheduler.Advanced;
using System.Threading.Tasks;
using System;
using FluentAssertions;

namespace InlineScheduler.Tests.Advanced
{    
    public class When_create_work_item
    {
        JobItem _item;

        [TestFixtureSetUp]
        public void Given_work_item()
        {
            var definition = JobFactory.Interval("Foo1", () =>
            {
                return Task.Factory.StartNew(() => Console.WriteLine("Foo1"));
            }, TimeSpan.FromMinutes(10));
            _item = new JobItem(new DefaultSchedulerContext(), definition, null);
        }

        [Test]
        public void Should_be_pending() 
        {
            _item.Status.Should().Be(JobStatus.Pending);                       
        }
    }
}
