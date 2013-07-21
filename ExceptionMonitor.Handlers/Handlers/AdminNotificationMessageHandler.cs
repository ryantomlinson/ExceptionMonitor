using NServiceBus;
using ExceptionMonitor.Domain;
using log4net;

namespace ExceptionMonitor.Handlers
{
	public class AdminNotificationMessageHandler : IHandleMessages<AdminNotificationMessage>
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(AdminNotificationMessage));
		public void Handle(AdminNotificationMessage message)
		{
			log.DebugFormat("New message received: {0}", message.Message);

			SignalRProxyConnectionManager.Send(message);
		}
	}
}