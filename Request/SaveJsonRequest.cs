using System.ComponentModel.DataAnnotations;

namespace backend.Request
{
    public class SaveJsonRequest
    {
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "JsonData is required.")]
        public string JsonData { get; set; }
    }
}
