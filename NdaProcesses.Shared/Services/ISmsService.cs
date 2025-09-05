using System.Threading.Tasks;
using NDAProcesses.Shared.Models;

namespace NDAProcesses.Shared.Services
{
    public interface ISmsService
    {
        Task<bool> SendSms(SmsMessage message);
    }
}
