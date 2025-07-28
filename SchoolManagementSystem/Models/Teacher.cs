using System.Collections.Generic;

namespace SchoolManagementSystem.Models
{
    public class Teacher : User
    {
        public string Qualification { get; set; }
        public string Specialization { get; set; }
        public DateTime JoinDate { get; set; }

        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}