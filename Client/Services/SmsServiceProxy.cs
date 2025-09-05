using NDAProcesses.Shared.Models;
using NDAProcesses.Shared.Services;
using System.Net.Http.Json;

namespace NDAProcesses.Client.Services
{
    public class SmsServiceProxy : ISmsService
    {
        private readonly HttpClient _httpClient;

        public SmsServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> SendSms(SmsMessage message)
        {
            var response = await _httpClient.PostAsJsonAsync("api/sms", message);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            return false;
        }
    }
}
