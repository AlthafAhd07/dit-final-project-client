
using static SkillsInternationalClient.Models.DTOs.AuthDTOs;

namespace SkillsInternationalClient.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginCredentialDto credentials);
    }
}
