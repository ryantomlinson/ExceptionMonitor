using Couchbase;

namespace ExceptionMonitor.Data
{
	public static class CouchbaseManager
	{
		private static readonly CouchbaseClient instance;

		static CouchbaseManager()
		{
			instance = new CouchbaseClient();
		}

		public static CouchbaseClient Instance { get { return instance; } }
	}
}