
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using SkillsInternationalClient.Models;
using SkillsInternationalClient.Utilities;

namespace SkillsInternationalClient.Services
{
    public class StudentService : IStudentService
    {
        private readonly HttpClient _httpClient;
        private readonly CredentialManager _credentialManager;

        public StudentService(HttpClient httpClient , CredentialManager credentialManager)
        {
            _credentialManager = credentialManager;

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:5000/");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _credentialManager.GetBasicAuthHeader());
        }


        public class StudentIdsData
        {
            public int[] StudentIds { get; set; } = [];
        }


        public class SingleStudentData
        {
            public Student? Student { get; set; }
        }

        public async Task<List<int>> GetAllStudentIdsAsync()
        {
            try
            {

            var response = await HttpUtils.FetchWithMinimumDelayAsync(_httpClient.GetAsync("/api/students/ids")) ;

            if (!response.IsSuccessStatusCode)
            {
                return [];
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            ApiResponse<StudentIdsData> apiResponse = JsonConvert.DeserializeObject<ApiResponse<StudentIdsData>>(jsonResponse);
            
                List<int>? studentIdList = apiResponse?.data?.StudentIds?.ToList();

                if (studentIdList == null)
                {
                    return new List<int>(); // Return an empty list
                }

                return studentIdList;

            }
            catch (Exception ex)
            {
                return [];
            }
        }

        public async Task<Student?> GetStudentByIdAsync(int regNo)
        {
            try
            {

                var response = await HttpUtils.FetchWithMinimumDelayAsync(_httpClient.GetAsync("/api/students/" + regNo));

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                ApiResponse<SingleStudentData> apiResponse = JsonConvert.DeserializeObject<ApiResponse<SingleStudentData>>(jsonResponse);

                Student? student = apiResponse?.data?.Student;

                if(student == null)
                {
                    return null;
                }

                return student;

            }
            catch (Exception ex)
            {
                return null;
            }

        }



        public async Task<Student?> CreateStudentAsync(Student student) {
            try {
                var serializedContent = JsonConvert.SerializeObject(student);
                var httpContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");

                var response = await HttpUtils.FetchWithMinimumDelayAsync(_httpClient.PostAsync("/api/students", httpContent));

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }


                string jsonResponse = await response.Content.ReadAsStringAsync();
                ApiResponse<SingleStudentData> apiResponse = JsonConvert.DeserializeObject<ApiResponse<SingleStudentData>>(jsonResponse);

                Student? newStudent = apiResponse?.data?.Student;

                if (student == null)
                {
                    return null;
                }

                return newStudent;




            } catch (Exception ex) {
                return null;
            }

    
        }

        public async Task<bool> DeleteStudentAsync(int regNO)
        {
            try {
                var response = await HttpUtils.FetchWithMinimumDelayAsync(_httpClient.DeleteAsync("/api/students/"+ regNO ));

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                return true;
            } catch (Exception ex) {

                return false;
            }

        }

        public async Task<bool> EditStudentAsync(Student student)
        {
            try {

                var serializedContent = JsonConvert.SerializeObject(student);
                var httpContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");


                var response = await HttpUtils.FetchWithMinimumDelayAsync(_httpClient.PutAsync("/api/students/" + student.RegNo , httpContent));

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                return true;
            } catch (Exception ex) {
                return false;
            }


        }

    }
}
