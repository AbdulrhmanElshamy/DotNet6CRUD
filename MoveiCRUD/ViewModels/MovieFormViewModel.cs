using MovieCRUD.Models;
using System.ComponentModel.DataAnnotations;

namespace MovieCRUD.ViewModels
{
    public class MovieFormViewModel
    {
        public int Id { get; set; }

        [Required,StringLength(250)]
        public string Title { get; set; }

        [Display(Name ="year")]
        public int year { get; set; }

        [Range(0,10)]
        public double Rate { get; set; }

        [Required,StringLength(2500)]
        public string StoreLine { get; set; }

        [Required,Display(Name ="Select Poster...")]
        public IFormFile File { get; set; }

        public string? ImgUrl { get; set; }

        [Display(Name ="Genre")]
        public int Genreid { get; set; }

        public IEnumerable<Genre>? Genres { get; set; }
    }
}
