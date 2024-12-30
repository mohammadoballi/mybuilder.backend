namespace backend.Model
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime? Birthday { get; set; }
        public string? ImagePath { get; set; }

        public bool isAdmin { get; set; } = false;
        public string? Career { get; set; }
    }
}
