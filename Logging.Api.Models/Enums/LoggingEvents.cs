using System;
using System.Collections.Generic;
using System.Text;

namespace Logging.Api.Models.Enums
{
    public enum LoggingEvents
    {
        GetAll = 1000,
        GetById,
        InsertRecord,
        UpdateRecord,
        DeleteRecord,

        GetItemNotFound = 2000,
        UpdateItemNotFound
    }
}
