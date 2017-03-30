using NServiceBus;

namespace Marriott.Conventions
{
    public static class EndpointConfigurationExtensions
    {
        public static void ConfigureCommonConfigurations(this EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.SendFailedMessagesTo("Marriott.Error");
            endpointConfiguration.AuditProcessedMessagesTo("Marriott.Audit");
        }
    }
}
