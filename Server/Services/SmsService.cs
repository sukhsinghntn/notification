using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using NDAProcesses.Shared.Models;
using NDAProcesses.Shared.Services;

namespace NDAProcesses.Server.Services
{
    public class SmsService : ISmsService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SmsService(HttpClient httpClient, IConfiguration configuration, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> SendSms(SmsMessage message)
        {
            var userName = _httpContextAccessor.HttpContext?.Request.Cookies["userName"];
            if (string.IsNullOrEmpty(userName))
            {
                return false;
            }

            var user = await _userService.GetUserData(userName);
            if (user == null)
            {
                return false;
            }

            var signature = $"\n\n- {user.DisplayName ?? user.UserName} ({user.Department})";

            var payload = new
            {
                content = message.Message + signature,
                encrypted = false,
                from = _configuration["Sms:From"],
                request_id = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"),
                to = message.PhoneNumber
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.httpsms.com/v1/messages/send");
            request.Headers.Add("x-api-Key", _configuration["Sms:ApiKey"]);
            var json = JsonSerializer.Serialize(payload);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
