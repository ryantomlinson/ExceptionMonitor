using System.Configuration;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using ExceptionMonitor.Domain;

namespace ExceptionMonitor.Handlers.ProxyConnections
{
	internal class AnalyticsProxyConnection
	{
		private static bool analyticsWebsiteConnected;
		private static IHubProxy analyticsWebsiteProxy;
		private static HubConnection analyticsWebsiteExceptionHubConnection;
		private static string analyticsWebsiteConnectionUrl = ConfigurationManager.AppSettings["AnalyticsWebsiteDashboardUrl"];

		public static void SendException(ExceptionMessage message)
		{
			if (!analyticsWebsiteConnected)
			{
				Connect();
			}

			if (analyticsWebsiteExceptionHubConnection.State == ConnectionState.Connected)
				analyticsWebsiteProxy.Invoke("SendException", message);
			else
			{
				Connect();
				analyticsWebsiteProxy.Invoke("SendException", message);
			}
		}

		private static void Connect()
		{
			analyticsWebsiteExceptionHubConnection = new HubConnection(analyticsWebsiteConnectionUrl);
			analyticsWebsiteProxy = analyticsWebsiteExceptionHubConnection.CreateHubProxy("ExceptionHub");
			analyticsWebsiteExceptionHubConnection.Start().Wait();
			analyticsWebsiteConnected = true;
		}
	}
}