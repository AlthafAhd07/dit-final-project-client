using System.Text;

namespace SkillsInternationalClient.Utilities
{
    public class CredentialManager
    {
        private string _username = "";
        private string _password = "";

        public void SaveCredentials(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public string GetBasicAuthHeader()
        {
            if (string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password))
            {
                throw new InvalidOperationException("Credentials are not set.");
            }

            var credentialBytes = Encoding.UTF8.GetBytes($"{_username}:{_password}");
            return Convert.ToBase64String(credentialBytes);
        }

        public void ClearCredentials()
        {
            _username = "";
            _password = "";
        }
    }

}
