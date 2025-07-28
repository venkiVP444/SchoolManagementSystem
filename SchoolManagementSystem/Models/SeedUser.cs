namespace SchoolManagementSystem.Models
{
    public class SeedUser
    {
        public int Id { get; set; }
        public string Role { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string? AdminRole { get; set; }
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }
        public string? Grade { get; set; }
        public string? ParentName { get; set; }
        public string? ParentContact { get; set; }
    }

}
