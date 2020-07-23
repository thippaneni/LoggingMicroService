using System.Threading.Tasks;
using Logging.Api.Models;

namespace Logging.Api.Services
{
    public interface ILoggingService
    {
        Task<LogResponseModel> LogAsync(LoggingModel logModel);
        Task<bool> HealthAsync();
    }
}
