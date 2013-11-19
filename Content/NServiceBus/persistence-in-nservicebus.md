<!--
title: "Persistence In NServiceBus"
tags: ""
summary: "Various features of NServiceBus require persistence. Among them are timeouts, sagas, and subscription storage."
-->

Various features of NServiceBus require persistence. Among them are timeouts, sagas, and subscription storage.

Four persistence technologies are in use in NServiceBus:

-   RavenDB
-   [NHibernate](persistence-in-nservicebus.md)
-   In Memory
-   MSMQ

Read about [installing Raven DB](using-ravendb-in-nservicebus-installing.md) and [how to connect to it](using-ravendb-in-nservicebus-connecting.md) .

Using NHibernate for persistence
--------------------------------

Starting with NServiceBus V3.0, [support for NHibernate persistence is now located in a separate assembly](relational-persistence-using-nhibernate.md) .

What's available?
-----------------

The following table summarizes what is available and how to configure each feature.

  -------------------- ---------- --------- ------------------------------------------------------------------------------------------------------ -------------------------------------
                       InMemory   RavenDB   [NHibernate](http://nservicebus.desk.com/customer/portal/articles/859337-persistence-in-nservicebus)   MSMQ
  Timeout              √          √         √                                                                                                      Not supported beginning with V3.3,0
  Subscription         √          √         √                                                                                                      √
  Saga                 √          √         √                                                                                                      
  Gateway              √          √         √                                                                                                      
  Distributor                               √                                                                                                      √
  Second Level Retry                                                                                                                               √
  Fault Management     √                                                                                                                           √
  Notifications                                                                                                                                    √
  -------------------- ---------- --------- ------------------------------------------------------------------------------------------------------ -------------------------------------

If self hosting, you can configure the persistence technology for each feature. For example, to store subscriptions in memory and timeouts in RavenDB, use this code:


```C#
        static void Main()
        {
            Configure.With()
            .Log4Net()
            .DefaultBuilder()
            .XmlSerializer()
            .MsmqTransport()
            .IsTransactional(true)
            .PurgeOnStartup(false)
            .InMemorySubscriptionStorage()
            .UnicastBus()
            .ImpersonateSender(false)
            .LoadMessageHandlers()
            .UseRavenTimeoutPersister()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());
        }

```


<p>


and for NServiceBus v4.x



```C#
        static void Main()
        {
            Configure.Serialization.Xml();

            Configure.Transactions.Enable();

            Configure.With()
            .Log4Net()
            .DefaultBuilder()
            .UseTransport<Msmq>()
            .PurgeOnStartup(false)
            .InMemorySubscriptionStorage()
            .UnicastBus()
            .RunHandlersUnderIncomingPrincipal(false)
            .LoadMessageHandlers()
            .UseRavenTimeoutPersister()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());
        }

```


</p> When you use NServiceBus.Host.exe out of the box, you can utilize one of the available profiles. The following table shows which persistence technology each pre-built profile configures by default. In addition, you can override the configured defaults. Read more about
[profiles](profiles-for-nservicebus-host.md) , [here too](more-on-profiles.md)
.

The following table summarizes the different persistence technologies being used by the built-in profiles. **NOTE** : Before configuring persistence technology, to avoid overriding your configurations, the profiles check if other types of storage are used.

  -------------------- ---------- ------------------------ ------------ ------------------------------
                       InMemory   RavenDB                  NHibernate   MSMQ

  Timeout              Lite       Integration/Production                Keeps a queue for management

  Subscription         Lite       Integration/Production                

  Saga                 Lite       Integration/Production                

  Gateway              Lite       MultiSite                             

  Distributor                                                           Distributor

  Second Level Retry                                                    Uses Timeout queue

  Fault Management     Lite                                             Integration/Production

  Notifications                                                         Lite/Integration/Production


  -------------------- ---------- ------------------------ ------------ ------------------------------

Default persisting technology
-----------------------------

The AsAServer role activates the timeout manager. This role does not explicitly determine which persisting technology to use. Hence, the default persisting technology for timeout manager (RavenDB) is used.

Similarly to the AsAServer role, the various profiles activate the different NServiceBus features, without explicitly configuring the persisting technology.

Read more about the [different profiles](more-on-profiles.md) .


