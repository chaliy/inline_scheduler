using NUnit.Framework;
using InlineScheduler.Advanced;
using FluentAssertions;

namespace InlineScheduler.Tests.Advanced
{
    public class When_work_item_is_ready_to_be_scheduled
    {
        WorkItem _item;

        [TestFixtureSetUp]
        public void Given_work_item() 
        {
            var factory = new WorkItemFactory();            
            _item = factory.Create();
            // Because work was never done before, work 
            // item should be ready right after createion...
            _item.UpdateState();
        }

        [Test]
        public void Should_be_scheduled() 
        {
            _item.Status.Should().Be(WorkStatus.Scheduled);                       
        }
    }
}
