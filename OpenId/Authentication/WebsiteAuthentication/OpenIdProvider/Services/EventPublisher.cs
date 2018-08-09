using SimpleBus.Core;

namespace WebSiteAuthentication.OpenIdProvider.Services
{
    public class EventPublisher : IEventPublisher
    {
        public void Publish<T>(T evt) where T : Event
        {
        }
    }
}
