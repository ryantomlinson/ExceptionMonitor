using System;
using NServiceBus;

namespace ExceptionMonitor.Domain
{
	[Serializable]
	public class AdminNotificationMessage : IMessage
	{
		public string Message			{ get; set; }
		public Country Country			{ get; set; }
		public DateTime TimeOccurred	{ get; set; }
		public NotificationType Type	{ get; set; }
	}

	public enum NotificationType
	{
		Verification,
		Info
	}
}