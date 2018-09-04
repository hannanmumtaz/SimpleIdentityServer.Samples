using SimpleBus.Core;

namespace WebsiteProtection.UmaProvider.Services
{
    public class DefaultEventPublisher : IEventPublisher
    {
        public void Publish<T>(T evt) where T : Event
        {
        }
    }
}
