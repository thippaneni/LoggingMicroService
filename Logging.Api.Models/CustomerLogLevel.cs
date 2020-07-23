using System;
using System.Collections.Generic;
using System.Text;
using Logging.Api.Models.Enums;

namespace Logging.Api.Models
{
    public class CustomerLogLevel
    {
        public LogsForCustomer CustomerId { get; set; }
        public ApiLogLevel LogLevel { get; set; }        
    }
}
