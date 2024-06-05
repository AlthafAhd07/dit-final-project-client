

namespace SkillsInternationalClient.Utilities
{

    public class ApiResponse<T>
    {
        public bool success { get; set; }
        public string? message { get; set; } = "Process completed successfully!";
        public T? data { get; set; }
        public Error? error { get; set; }
    }

    public class Error
    {
        public string message { get; set; }
        public string code { get; set; }
    }


    public class HttpUtils
    {
        // I am using this to avoid flikering in loading animation when server respond too fast
        public static async Task<HttpResponseMessage> FetchWithMinimumDelayAsync(Task<HttpResponseMessage> fetchTask, int seconds = 500)
        {
            // Create a delay task for the minimum delay
            var delayTask = Task.Delay(seconds);

            await Task.WhenAll(fetchTask, delayTask);
        
            return await fetchTask;
        }

    }

}
