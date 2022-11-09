using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieCRUD.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required, MaxLength(250)]
        public string Title { get; set; }

        public int year { get; set; }

        public double Rate { get; set; }

        [Required,MaxLength(2500)]
        public string StoreLine { get; set; }

        [Required]
        public string ImgUrl { get; set; }

        public int GenreId { get; set; }

        [ForeignKey("GenreId")]
        public virtual Genre Genre { get; set; }

    }
}
