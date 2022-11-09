using System.ComponentModel.DataAnnotations;

namespace MovieCRUD.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required, MaxLength(250)]
        public string Name { get; set; }

        public virtual IEnumerable<Movie> ? Movies { get; set; }  
    }
}
