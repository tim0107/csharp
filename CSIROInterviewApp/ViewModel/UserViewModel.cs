using CSIROInterviewApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CSIROInterviewApp.ViewModel
{
    public class UserViewModel
    {
        public float? GPA { get; set; }
        public List<UserDetail> Users { get; set; }
    }

    public class UserDetail
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Course { get; set; }

        public float GPA { get; set; }

        public string University { get; set; }

        public string CoverLetterFilePath { get; set; }
        public string ResumeFilePath { get; set; }
    }
}
