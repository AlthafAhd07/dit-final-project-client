using static SkillsInternationalClient.Models.DTOs.AuthDTOs;
using SkillsInternationalClient.Services;
using SkillsInternationalClient.Controls;
using SkillsInternationalClient.Utilities;


namespace SkillsInternationalClient
{
    public partial class LoginForm : Form
    {

        private readonly AuthService _authService;
        LoadingOverlay? loading;


        public LoginForm(AuthService authService)
        {
            _authService = authService;

            InitializeComponent();


            this.Resize += Form_Resize;

            Form_Resize(this, EventArgs.Empty);

            loading = new LoadingOverlay(this);

        }

        private void Form_Resize(object? sender, EventArgs e)
        {
            CenterGroupBox();
            HorizontallyCenterTitleLogo();
        }

        private void CenterGroupBox()
        {
            int groupboxWidth = LoginGroupBox.Width;
            int groupboxHeight = LoginGroupBox.Height;
            int formWidth = this.ClientSize.Width; // Get the form's inner width
            int formHeight = this.ClientSize.Height; // Get the form's inner height

            LoginGroupBox.Location = new Point(
                (formWidth - groupboxWidth) / 2,
                (formHeight - groupboxHeight) / 2 + 50);
        }

        private void HorizontallyCenterTitleLogo()
        {
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            int logoWidth = LogoPictureBox.Width;
            int logoHeight = LogoPictureBox.Height;

            int brandNameWidth = BrandName.Width;
            int brandNameHeight = BrandName.Height;

            int groupboxTop = (formHeight - LoginGroupBox.Height) / 2 + 50;


            LogoPictureBox.Location = new Point((formWidth - logoWidth) / 2, groupboxTop - logoHeight - brandNameHeight - 35);
            BrandName.Location = new Point((formWidth - brandNameWidth) / 2, groupboxTop - brandNameHeight - 15);

        }




        private async void btnLogin_Click(object sender, EventArgs e)
        {

            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username and password." , "Error");
                return;
            }


            LoginCredentialDto credentials = new LoginCredentialDto
            {
                Username = username,
                Password = password
            };

            loading?.ShowOverlay("Authenticating... Please wait.");


            bool isAuthenticated = await _authService.LoginAsync(credentials);

            loading?.HideOverlay();

            if (!isAuthenticated)
            {
                 MessageBox.Show("Invalid credentials. Please try again.", "Invalid login Details");

                return;
            }

            // Close the current form (the login form)
            this.Hide();



            HttpClient httpClient = new HttpClient();
            CredentialManager credentialManager = new CredentialManager();
            credentialManager.SaveCredentials(username, password);
            StudentService studentService = new StudentService(httpClient , credentialManager);

            // Create the StudentForm
            StudentForm studentForm = new StudentForm(studentService);

            // Set the location of the new form to match the old one
            studentForm.StartPosition = FormStartPosition.Manual;
            studentForm.Location = this.Location;
            studentForm.Size = this.Size; // Optional: Match size too 

            // Show the StudentForm
            studentForm.Show();
        }

        private void btnClearLogin_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtPassword.Text = "";

            txtUsername.Focus();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }


    }
}
