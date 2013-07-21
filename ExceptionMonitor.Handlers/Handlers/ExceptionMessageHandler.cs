using NServiceBus;
using ExceptionMonitor.Domain;
using log4net;

namespace ExceptionMonitor.Handlers
{
    public class ExceptionMessageHandler : IHandleMessages<ExceptionMessage>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ExceptionMessageHandler));
        public void Handle(ExceptionMessage message)
        {
            log.DebugFormat("New message received: {0}", message.Message);

			SignalRProxyConnectionManager.Send(message);
        }
    }
}
