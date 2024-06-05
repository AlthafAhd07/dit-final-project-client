

namespace SkillsInternationalClient.Models
{

    public class StudentContact
    {
        public string Address { get; set; } = "";
        public string Email { get; set; } = "";
        public string MobilePhone { get; set; } = "";
        public string HomePhone { get; set; } = "";
    }

    public class StudentParent
    {
        public string Name { get; set; } = "";

        public string Nic { get; set; } = "";
        public string ContactNumber { get; set; } = "";
    }

    public class Student
    {
        public int? RegNo{ get; set; }

        public string FirstName { get; set;} = "";

        public string LastName { get; set; } = "";

        public string Dob { get; set; } = "";

        public string Gender { get; set; } = "";


        public StudentContact Contact { get; set; }

        public StudentParent Parent { get; set; }

    }
}
