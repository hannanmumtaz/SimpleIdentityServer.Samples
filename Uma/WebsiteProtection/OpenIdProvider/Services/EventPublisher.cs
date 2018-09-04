using SimpleBus.Core;

namespace WebsiteProtection.OpenIdProvider.Services
{
    public class EventPublisher : IEventPublisher
    {
        public void Publish<T>(T evt) where T : Event
        {
        }
    }
}
