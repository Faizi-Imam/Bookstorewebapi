using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Models
{
    public class BookModel
    {

        public int id { get; set; }
        [Required(ErrorMessage ="Please add the title")]
        public string? Title { get; set; }

        public string? Description { get; set; }
    }
}
