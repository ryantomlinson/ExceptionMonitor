using System;
using NServiceBus;

namespace ExceptionMonitor.Domain
{
	[Serializable]
	public class RegistrationMessage : IMessage, IAnalyticsEntity
	{
		public int UserId { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PostCode { get; set; }
		public Country Country { get; set; }
	}
}