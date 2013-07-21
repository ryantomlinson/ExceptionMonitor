using System;
using ExceptionMonitor.Data.Entities;

namespace ExceptionMonitor.Data.Repositories
{
	public class RegistrationsRepository : BaseRepository<Registration>
	{
		 public Registration GetRegistrations()
		 {
			 try
			 {
				 return Get<Registration>(StoreKey.Registrations);
			 }
			 catch (Exception)
			 {
				 
				 throw;
			 }
		 }

		public bool Store(Registration registration)
		{
			try
			{
				return base.Store(registration, StoreKey.Registrations);
			}
			catch (Exception)
			{
				
				throw;
			}
		}
	}
}