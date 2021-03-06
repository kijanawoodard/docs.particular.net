using System;
using NServiceBus;

public class MessageConventions
{
    public void Simple()
    {
        #region MessageConventionsV5

        var configure = Configure.With(b => b.Conventions(c =>
        {
            c.DefiningCommandsAs(t => t.Namespace == "MyNamespace" && t.Namespace.EndsWith("Commands"));
            c.DefiningEventsAs(t => t.Namespace == "MyNamespace" && t.Namespace.EndsWith("Events"));
            c.DefiningMessagesAs(t => t.Namespace == "Messages");
            c.DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"));
            c.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));
            c.DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));
            c.DefiningTimeToBeReceivedAs(t => t.Name.EndsWith("Expires") ? TimeSpan.FromSeconds(30) : TimeSpan.MaxValue);
        }));

        #endregion
    }

}