using System;
using Enyim.Caching.Memcached;
using ExceptionMonitor.Data.Entities;

namespace ExceptionMonitor.Data.Repositories
{
	public abstract class BaseRepository<TDocument> where TDocument : class, IDocument
	{
		public TDocument Get<TDocument>(StoreKey storeKey) where TDocument : class, IDocument
		{
			try
			{
				var client = CouchbaseManager.Instance;

				var data = client.Get<TDocument>(ResolveStorageKey(storeKey));

				return data;
			}
			catch (Exception)
			{
				
				throw;
			}
		}

		public bool Store(IDocument document, StoreKey storeKey)
		{
			try
			{
				var client = CouchbaseManager.Instance;

				return client.Store(StoreMode.Set, ResolveStorageKey(storeKey), document);

			}
			catch (Exception)
			{
				
				throw;
			}
		}

		private string ResolveStorageKey(StoreKey storeKey)
		{
			return string.Format("Key_{0}", storeKey);
		}
	}
}