using Marriott.Conventions;
using Marriott.ITOps.PaymentGateway;

namespace Marriott.Business.Endpoint
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            //MIKE: this is to suport request/response using .Request<T>, but I think we also need it to supprt requets/reply, which is async
            endpointConfiguration.MakeInstanceUniquelyAddressable("Marriott.Business.Endpoint");
            endpointConfiguration.ConfigureCommonConfigurations();
            endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<CreditCardService>(DependencyLifecycle.InstancePerCall));
        }
    }
}
