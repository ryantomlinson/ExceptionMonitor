using NServiceBus;
using ExceptionMonitor.Domain;
using ExceptionMonitor.Handlers.ProxyConnections;

namespace ExceptionMonitor.Handlers
{
	internal static class SignalRProxyConnectionManager
    {
        public static void Send<T>(T message) where T : class, IMessage
        {
	        if (message is ExceptionMessage)
				AnalyticsProxyConnection.SendException(message as ExceptionMessage);

			if (message is AdminNotificationMessage)
				UkAdminProxyConnection.SendNotification(message as AdminNotificationMessage);
        }
    }
}