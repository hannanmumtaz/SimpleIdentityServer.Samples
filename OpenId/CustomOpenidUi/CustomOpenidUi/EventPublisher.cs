using SimpleBus.Core;

namespace CustomOpenidUi
{
    public class EventPublisher : IEventPublisher
    {
        public void Publish<T>(T evt) where T : Event
        {
        }
    }
}
