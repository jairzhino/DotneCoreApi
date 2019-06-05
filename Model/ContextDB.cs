using Microsoft.EntityFrameworkCore;

namespace Backend.Model{
    public class ContextDB : DbContext
    {
        public ContextDB(DbContextOptions<ContextDB> options)
            : base(options)
        {
        }

        public DbSet<Country> countries {get;set;}
        public DbSet<City> cities{get;set;}
    }
}