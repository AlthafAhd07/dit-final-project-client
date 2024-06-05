using System.Text;
using Newtonsoft.Json;
using static SkillsInternationalClient.Models.DTOs.AuthDTOs;
using SkillsInternationalClient.Utilities;

namespace SkillsInternationalClient.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:5000/");
        }

        public async Task<bool> LoginAsync(LoginCredentialDto credentials)
        {
            try
            {
                var serializedContent = JsonConvert.SerializeObject(credentials);
                var httpContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");

                var response = await HttpUtils.FetchWithMinimumDelayAsync(_httpClient.PostAsync("/api/auth/login", httpContent));

                if(!response.IsSuccessStatusCode) {

                    return false;
                }

                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }
    }
}
