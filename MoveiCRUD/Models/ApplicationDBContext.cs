using Microsoft.EntityFrameworkCore;
using MovieCRUD.Models;

namespace MovieCRUD.Models
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options)
        {

        }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Movie> Movies { get; set; } 
    }
}
