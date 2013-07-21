using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExceptionMonitor.Data.Entities;
using ExceptionMonitor.Data.Repositories;

namespace ExceptionMonitor.Data.Test
{
	class Program
	{
		private static void Main(string[] args)
		{
			RegistrationsRepository repository = new RegistrationsRepository();

			Registration registration = new Registration();
			registration.DailyCount = 1;

			repository.Store(registration);

			var reg = repository.GetRegistrations();
			Console.WriteLine("Registrations: {0}", reg.DailyCount);
			Console.ReadKey();
		}

	}
}
