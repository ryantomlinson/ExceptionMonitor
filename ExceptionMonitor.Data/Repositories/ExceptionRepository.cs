using System;
using Couchbase;
using Enyim.Caching.Memcached;

namespace ExceptionMonitor.Data.Repositories
{
	public class ExceptionRepository
	{
		//TODO: Yes this isn't testible or nice.
		protected readonly CouchbaseClient client = null;

		public ExceptionRepository()
		{
			try
			{
				client = new CouchbaseClient();
			}
			catch (Exception)
			{
				client = null;
			}
		}

		public bool IsRunning
		{
			get
			{
				return client != null;
			}
		}

		public void Add<T>(StoreKey type, string key, T value)
		{
			if (client != null)
				client.Store(StoreMode.Set, ResolveCacheKey(type, key), value);
		}

		public T Get<T>(StoreKey storeKey, string key)
		{
			if (client == null)
				return default(T);

			return client.Get<T>(ResolveCacheKey(storeKey, key));
		}

		private static string ResolveCacheKey(StoreKey storeKey, string key)
		{
			return String.Format("{0}_{1}", storeKey, key);
		}
	}
}