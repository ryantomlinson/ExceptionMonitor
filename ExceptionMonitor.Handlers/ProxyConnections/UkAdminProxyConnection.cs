using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web.Security;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using ExceptionMonitor.Domain;

namespace ExceptionMonitor.Handlers.ProxyConnections
{
	internal class UkAdminProxyConnection
	{
		private static bool adminWebsiteConnected;
		private static IHubProxy adminWebsiteProxy;
		private static HubConnection adminWebsiteExceptionHubConnection;
		private static string adminWebsiteConnectionUrl = ConfigurationManager.AppSettings["AdminWebsiteDashboardUrl"];

		public static void SendNotification(AdminNotificationMessage message)
		{
			if (!adminWebsiteConnected)
			{
				Connect();
			}

			if (adminWebsiteExceptionHubConnection.State == ConnectionState.Connected)
				adminWebsiteProxy.Invoke("SendNotification", message);
			else
			{
				Connect();
				adminWebsiteProxy.Invoke("SendNotification", message);
			}
		}

		private static Cookie GetAuthCookie(string user, string pass)
		{
			var http = WebRequest.Create("https://localhost/new/admin/Login/Index") as HttpWebRequest;
			http.UserAgent = "Mozilla/5.0";
			http.AllowAutoRedirect = false;
			http.Method = "POST";
			http.ContentType = "application/x-www-form-urlencoded";
			http.CookieContainer = new CookieContainer();
			var postData = "UserName=" + user + "&Password=" + pass;
			byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(postData);
			http.ContentLength = dataBytes.Length;
			using (var postStream = http.GetRequestStream())
			{
				postStream.Write(dataBytes, 0, dataBytes.Length);
			}

			try
			{
				var httpResponse = http.GetResponse() as HttpWebResponse;
				return httpResponse.Cookies[FormsAuthentication.FormsCookieName];
			} 
			catch(Exception e) {
			   if (e is WebException && ((WebException)e).Status==WebExceptionStatus.ProtocolError)
			   {
				  WebResponse errResp = ((WebException)e).Response;
				  using(Stream respStream = errResp.GetResponseStream())
				  {
					 StreamReader reader = new StreamReader(respStream);
					  var res = reader.ReadToEnd();
				  }
			   }
			}
			return null;

		}

		private static void Connect()
		{
			ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
			adminWebsiteExceptionHubConnection = new HubConnection(adminWebsiteConnectionUrl)
				{
					CookieContainer = new CookieContainer()
				};
			adminWebsiteExceptionHubConnection.CookieContainer.Add(GetAuthCookie("ryan.tomlinson", "giprF-dhC1j3JtODAb+l"));
			adminWebsiteProxy = adminWebsiteExceptionHubConnection.CreateHubProxy("AdminNotificationHub");
			try
			{
				adminWebsiteExceptionHubConnection.Start().Wait();
			}
			catch (Exception ex)
			{
				var error = ex.GetError();
				throw;
			}

			adminWebsiteConnected = true;
		}
	}
}