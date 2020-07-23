using Logging.Api.Models.Enums;

namespace Logging.Api.Models
{
    public class LogResponseModel
    {
        public string Id { get; set; }
        public LogStatus Status { get; set; }
    }
}
