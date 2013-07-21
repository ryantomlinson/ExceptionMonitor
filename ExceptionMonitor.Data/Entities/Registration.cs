using System;

namespace ExceptionMonitor.Data.Entities
{
	[Serializable]
	public class Registration : IDocument
	{
		public int DailyCount { get; set; }
		//public int Id { get; private set; }
		//public string Type { get; set; }
		//public string Key { get; set; }
		//public string GetKey()
		//{
		//	throw new System.NotImplementedException();
		//}

		//public void Init()
		//{
		//	throw new System.NotImplementedException();
		//}

		//public void AfterSave()
		//{
		//	throw new System.NotImplementedException();
		//}
	}
}