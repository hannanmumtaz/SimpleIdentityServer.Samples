using SimpleBus.Core;

namespace BasicAccountFilter.Services
{
    public class EventPublisher : IEventPublisher
    {
        public void Publish<T>(T evt) where T : Event
        {
        }
    }
}
