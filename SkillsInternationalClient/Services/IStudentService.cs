using SkillsInternationalClient.Models;

namespace SkillsInternationalClient.Services
{
    internal interface IStudentService
    {
        Task<List<int>> GetAllStudentIdsAsync();
        Task<Student?> GetStudentByIdAsync(int regNo);

        Task<Student> CreateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int regNO);
        Task<bool> EditStudentAsync(Student student);
    }
}
