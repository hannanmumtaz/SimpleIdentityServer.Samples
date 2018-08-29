using SimpleBus.Core;

namespace ManageScimResources.OpenIdProvider.Services
{
    public class EventPublisher : IEventPublisher
    {
        public void Publish<T>(T evt) where T : Event
        {
        }
    }
}
