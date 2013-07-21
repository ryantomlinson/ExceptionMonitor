using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client.Hubs;
using NServiceBus;
using ExceptionMonitor.Domain;

namespace ExceptionMonitor.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {

            Random randomCountry = new Random();
            var bus = Configure.With()
                .Log4Net()
                .DefaultBuilder()
                .MsmqTransport()
                .MsmqSubscriptionStorage()
                .XmlSerializer()
                .DisableRavenInstall()
                .DisableTimeoutManager()
                .UnicastBus()
                .SendOnly();

			Console.WriteLine("Please select an option:");
			Console.WriteLine("Type 1: For exception testing");
			Console.WriteLine("Type 2: For admin testing"); 

            while (true) 
            {
                string line = Console.ReadLine();

                if (line == "exit")
                {
                    break;
                }
	            if (line == "1")
	            {
		            try
		            {
			            throw new ArgumentException("Some exception happened!" + line);
		            }
		            catch (Exception exception)
		            {
			            var message = new ExceptionMessage()
				            {
					            Message = exception.Message,
					            Country = (Country) randomCountry.Next(0, 3),
					            //Country = Country.Uk,
					            StackTrace = exception.StackTrace,
					            TimeOccurred = DateTime.Now,
					            Type = (ExceptionType) randomCountry.Next(0, 2),
				            };
			            bus.Send(message);
		            }
	            }
				if (line == "2")
				{
					Console.WriteLine("Enter test message");
					var message = Console.ReadLine();
					var adminMessage = new AdminNotificationMessage()
						{
							Country = Country.Uk,
							Message = message,
							TimeOccurred = DateTime.Now,
							Type = NotificationType.Verification
						};
					bus.Send(adminMessage);
				}
            }
        }
    }
}
