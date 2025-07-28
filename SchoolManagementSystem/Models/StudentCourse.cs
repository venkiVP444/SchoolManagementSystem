﻿namespace SchoolManagementSystem.Models
{
    public class StudentCourse
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public Student Student { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public decimal? Grade { get; set; }
    }
}