using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Models
{
    public class EnrollmentViewModel
    {
        [Required]
        public string StudentId { get; set; }

        [Required]
        public int CourseId { get; set; } 

        public List<Student> Students { get; set; }
        public List<Course> Courses { get; set; }
    }
}