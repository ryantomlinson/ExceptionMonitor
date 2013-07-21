using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ExceptionMonitor.Domain;

namespace ExceptionMonitor.Dashboard.Hubs
{
    [HubName("exceptionHub")]
    public class Exceptionhub : Hub
    {
        public void SendException(ExceptionMessage exception)
        {
            Clients.All.displayException(exception);
        }
        
    }
}