
using SkillsInternationalClient.Models;
using SkillsInternationalClient.Services;
using SkillsInternationalClient.Controls;

namespace SkillsInternationalClient
{
    public partial class StudentForm : Form
    {

        private readonly StudentService _studentService;
        List<int> _studentIds = new List<int>();
        private int? _selectedRegNo = null;
        private Student? _selectedStudent = null;
        LoadingOverlay? loading;

        public StudentForm(StudentService studentService)
        {
            _studentService = studentService;
            InitializeComponent();
            HandleButtonsVisibility(false);

            loading = new LoadingOverlay(this);
        }

        private async void StudentForm_Load(object sender, EventArgs e)
        {

            loading?.ShowOverlay("Fething student registration numbers...");

            List<int> studentIds = await _studentService.GetAllStudentIdsAsync();

            loading?.HideOverlay();

            ComboRegNo.Items.Clear();
            foreach (int studentId in studentIds)
            {
                ComboRegNo.Items.Add(studentId);
            }

            _studentIds = studentIds;

            HandleStudentCountWording(_studentIds.Count);

        }


        private void HandleStudentCountWording(int count = 0)
        {

            string labelTxt = "( No students found. Create a student. )";

            if (count > 0)
            {
                string studentWording = "student";
                if(count > 1) {
                    studentWording = "students";
                }
                labelTxt = $"( {count} {studentWording} found )";
            }

            LabelStudentCount.Text = labelTxt;
        }

        private Student GetStudentDataInput()
        {
            string regNo = ComboRegNo.Text;


            var student = new Student();

            if (!string.IsNullOrEmpty(regNo))
            {
                if (int.TryParse(regNo, out int regNoInt))
                {
                    student.RegNo = regNoInt;
                }
            }



            student.FirstName = TxtFirstName.Text;
            student.LastName = TxtLastName.Text;
            student.Dob = TxtDob.Text;



            if (radioMale.Checked)
            {
                student.Gender = "Male";
            }
            else if (radioFemale.Checked)
            {
                student.Gender = "Female";
            }


            StudentContact Contact = new StudentContact();


            Contact.Address = TxtAddress.Text;
            Contact.Email = TxtEmail.Text;
            Contact.MobilePhone = TxtMobilePhone.Text;
            Contact.HomePhone = TxtHomePhone.Text;

            student.Contact = Contact;


            StudentParent Parent = new StudentParent();



            Parent.Name = TxtParentName.Text;
            Parent.Nic = TxtNic.Text;
            Parent.ContactNumber = TxtParentContactNumber.Text;

            student.Parent = Parent;


            return student;

        }

        private static bool ValidateStudentData(Student? student)
        {
            if (student == null)
            {
                return false;
            }
            // Validate core student properties
            if (student.RegNo == null)
            {
                return false;
            }


            if (string.IsNullOrWhiteSpace(student.FirstName)) return false;
            if (string.IsNullOrWhiteSpace(student.LastName)) return false;
            if (string.IsNullOrWhiteSpace(student.Dob)) return false;
            if (string.IsNullOrWhiteSpace(student.Gender)) return false;

            if (student.Contact == null) return false;
            if (string.IsNullOrWhiteSpace(student.Contact.Address)) return false;
            if (string.IsNullOrWhiteSpace(student.Contact.Email)) return false;
            if (string.IsNullOrWhiteSpace(student.Contact.MobilePhone)) return false;
            if (string.IsNullOrWhiteSpace(student.Contact.HomePhone)) return false;

            if (student.Parent == null) return false;
            if (string.IsNullOrWhiteSpace(student.Parent.Name)) return false;
            if (string.IsNullOrWhiteSpace(student.Parent.ContactNumber)) return false;
            if (string.IsNullOrWhiteSpace(student.Parent.Nic)) return false;

            return true;
        }

        private void ClearSelectedStudentData()
        {
            TxtFirstName.Text = "";
            TxtLastName.Text = "";
            TxtDob.Text = "";

            TxtEmail.Text = "";
            TxtAddress.Text = "";
            TxtMobilePhone.Text = "";
            TxtHomePhone.Text = "";

            radioMale.Checked = false;
            radioFemale.Checked = false;

            TxtParentName.Text = "";
            TxtNic.Text = "";
            TxtParentContactNumber.Text = "";


            _selectedStudent = null;

            HandleButtonsVisibility(false);

        }

        private void SetSelectedStudentData(Student? student = null)
        {
            _selectedRegNo = student?.RegNo ?? null;
            _selectedStudent = student;

            // Handle null student object
            if (student == null)
            {
                ClearSelectedStudentData();
                return;
            }

            TxtFirstName.Text = student.FirstName;
            TxtLastName.Text = student.LastName;
            TxtDob.Text = student.Dob;

            TxtEmail.Text = student.Contact.Email;
            TxtAddress.Text = student.Contact.Address;
            TxtMobilePhone.Text = student.Contact.MobilePhone;
            TxtHomePhone.Text = student.Contact.HomePhone;

            radioMale.Checked = student.Gender == "Male";
            radioFemale.Checked = student.Gender == "Female";

            TxtParentName.Text = student.Parent.Name;
            TxtNic.Text = student.Parent.Nic;
            TxtParentContactNumber.Text = student.Parent.ContactNumber;

        }


        private void HandleButtonsVisibility(bool isStudentExists = false)
        {

            if (!isStudentExists)
            {
                ButtonRegister.Show();
                ButtonRegister.Location = new Point(703, 765);


                ButtonUpdate.Hide();
                ButtonDelete.Hide();

                return;
            }


            ButtonRegister.Hide();

            ButtonUpdate.Show();
            ButtonUpdate.Location = new Point(703, 765);

            ButtonDelete.Show();
            ButtonDelete.Location = new Point(20, 765);




        }

        private void ComboRegNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void ComboRegNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            object? selectedItem = ComboRegNo.SelectedItem;

            HandleRegNoChange(selectedItem?.ToString() ?? null);

        }

        private void ComboRegNo_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {

                string enteredText = ComboRegNo.Text;

                HandleRegNoChange(enteredText);

                e.SuppressKeyPress = true;  // Prevent further processing and the sound
            }
        }


        private void ComboRegNo_Leave(object sender, EventArgs e)
        {
            string regNo = ComboRegNo.Text;

            HandleRegNoChange(regNo);

        }


        private async void HandleRegNoChange(string? regNoString)
        {
            if (regNoString == null)
            {
                ClearSelectedStudentData();
                return;
            }

            if (string.IsNullOrEmpty(regNoString))
            {
                ClearSelectedStudentData();
                return;
            }

            if (!int.TryParse(regNoString, out int regNo))
            {
                ClearSelectedStudentData();
                return;
            }

            if (regNo == _selectedRegNo)
            {
                return;
            }


            _selectedRegNo = regNo;

            bool isStudentExists = _studentIds.Contains(regNo);

            if (!isStudentExists)
            {
                HandleButtonsVisibility(false);
                ClearSelectedStudentData();

                return;
            }



            loading?.ShowOverlay("Retrieving student data...");

            _selectedStudent = await _studentService.GetStudentByIdAsync(regNo);

            loading.HideOverlay();

            SetSelectedStudentData(_selectedStudent);

            HandleButtonsVisibility(_selectedStudent != null);


        }



        private void BtnExit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void LogoutLinkLable_Clicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            // Close the current form (the login form)
            this.Hide();

            HttpClient httpClient = new HttpClient();
            AuthService authService = new AuthService(httpClient);

            // Create the StudentForm
            LoginForm loginForm = new LoginForm(authService);

            // Set the location of the new form to match the old one
            loginForm.StartPosition = FormStartPosition.Manual;
            loginForm.Location = this.Location;
            loginForm.Size = this.Size; // Optional: Match size too 

            loginForm.Show();
        }


        private void ButtonClear_Click(object sender, EventArgs e)
        {
            _selectedRegNo = null;

            ClearSelectedStudentData();

            ComboRegNo.Text = "";
            ComboRegNo.SelectedIndex = -1;

            ComboRegNo.Focus();
        }

        private async void ButtonRegister_Click(object sender, EventArgs e)
        {
            Student student = GetStudentDataInput();

            // when registering a student regNo will not be considered it will be managed by db auto increment
            if (student.RegNo == null)
            {
                student.RegNo = 0;
            }

            bool isValidData = ValidateStudentData(student);


            if (!isValidData)
            {
                MessageBox.Show("Please fill all fields", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            loading?.ShowOverlay("Registering student...");

            Student? newStudent = await _studentService.CreateStudentAsync(student);

            loading?.HideOverlay();

            bool isStudentExists = newStudent != null;

            SetSelectedStudentData(newStudent);

            HandleButtonsVisibility(isStudentExists);

            if (isStudentExists)
            {
                int regNo = newStudent?.RegNo ?? 0;

                ComboRegNo.Items.Add(regNo);
                ComboRegNo.Text = regNo.ToString();

                _studentIds.Add(regNo);
                HandleStudentCountWording(_studentIds.Count);

                MessageBox.Show("Student created successfully!", "Success");
            }
            else
            {

                MessageBox.Show("Something went wrong in student creation. Please try again." , "System Error");

            }


        }

        private async void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (_selectedRegNo == null || _selectedRegNo == 0)
            {
                MessageBox.Show("Please select a student to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int regNo = _selectedRegNo ?? 0;

            // Confirmation Dialog
            var confirmResult = MessageBox.Show(
                $"Are you sure you want to delete the student with registration number {_selectedRegNo}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes)
            {
                return; // User chose not to delete
            }

            loading?.ShowOverlay("Deleting student record...");

            bool isDeleted = await _studentService.DeleteStudentAsync(_selectedRegNo ?? 0);

            loading?.HideOverlay();

            if (!isDeleted)
            {
                MessageBox.Show("Something went wrong in student creation. Please try again.", "System Error");
                HandleButtonsVisibility(true);

                return;

            }

            HandleButtonsVisibility(false);
            SetSelectedStudentData(null);


            ComboRegNo.Items.Remove(regNo);
            ComboRegNo.Text = "";

            _studentIds.Remove(regNo);
            HandleStudentCountWording(_studentIds.Count);


            MessageBox.Show("Student deleted successfully!");
        }

        private async void ButtonUpdate_Click(object sender, EventArgs e)
        {
            Student student = GetStudentDataInput();

            bool isValidData = ValidateStudentData(student);


            if (!isValidData)
            {
                MessageBox.Show("Please fill all fields", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            loading?.ShowOverlay("Updating student information...");

            bool isUpdated = await _studentService.EditStudentAsync(student);

            loading?.HideOverlay();

            if (!isUpdated)
            {
                MessageBox.Show("Something went wrong in student creation. Please try again.", "System Error");
                return;
            }

            ComboRegNo.Focus();
            MessageBox.Show("Student updated successfully!");
        }

    }
}
