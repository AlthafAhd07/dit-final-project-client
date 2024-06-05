using SkillsInternationalClient.Services;

namespace SkillsInternationalClient
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            HttpClient httpClient = new HttpClient();
            AuthService authService = new AuthService(httpClient);


             Application.Run(new LoginForm(authService));

        }
    }
}
