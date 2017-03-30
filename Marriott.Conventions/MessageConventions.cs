using System;
using NServiceBus;

namespace Marriott.Conventions
{
    public class MessageConventions : INeedInitialization
    {
        public void Customize(EndpointConfiguration configuration)
        {
            configuration.Conventions()
                .DefiningCommandsAs(t => IsFromMessageAssembly(t) && t.Namespace.EndsWith("Commands"))
                .DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t) || (t.Namespace != null && t.Namespace.EndsWith(".Events")) || t.Assembly.GetName().Name.Contains(".Events"))
                .DefiningMessagesAs(t => IsFromMessageAssembly(t) && t.Namespace.EndsWith("Messages"));
        }

        private static bool IsFromMessageAssembly(Type t)
        {
            return t.Namespace != null
                && !t.Namespace.StartsWith("NServiceBus.")
                && !t.Namespace.StartsWith("System.");
        }
    }
}