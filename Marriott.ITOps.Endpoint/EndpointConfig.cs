using Marriott.Conventions;
using Marriott.ITOps.Notifications.Email;
using Marriott.ITOps.PaymentGateway;

namespace Marriott.ITOps.Endpoint
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration.ConfigureCommonConfigurations();
            endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<EmailSender>(DependencyLifecycle.InstancePerCall));
            endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<CreditCardService>(DependencyLifecycle.InstancePerCall));
        }
    }
}
