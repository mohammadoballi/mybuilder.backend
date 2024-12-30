using System.ComponentModel.DataAnnotations;

namespace backend.Model
{
    public class UserJsonData
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FilePath { get; set; }

        public User User { get; set; } 
    }
}
