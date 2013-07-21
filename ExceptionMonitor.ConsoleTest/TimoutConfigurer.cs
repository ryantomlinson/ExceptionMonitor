using NServiceBus;
using NServiceBus.Config;

namespace ExceptionMonitor.ConsoleTest
{
    public class TimeoutConfigurer : INeedInitialization
    {
        public void Init()
        {
            Configure.Instance.UseInMemoryTimeoutPersister();
        }
    }
}