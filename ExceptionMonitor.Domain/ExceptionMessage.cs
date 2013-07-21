using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace ExceptionMonitor.Domain
{
    [Serializable]
	public class ExceptionMessage : IMessage, IAnalyticsEntity
    {
        public string Message { get; set; }
        public Country Country { get; set; }
        public DateTime TimeOccurred { get; set; }
        public string StackTrace { get; set; }
        public ExceptionType Type { get; set; }
    }

    public enum ExceptionType
    {
        Unhandled,
        Software
    }
}
