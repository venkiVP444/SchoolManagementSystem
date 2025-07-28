using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Models
{
    public class Student : User
    {
        public string Grade { get; set; }
        public string ParentName { get; set; }
        public string ParentContact { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();

    }
}