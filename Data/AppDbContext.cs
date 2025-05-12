
using AuthApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { } // cria o do construtor de DbContext

        public DbSet<User> Users => Set<User>(); // cria um DbSet de Users que é uma tabela no banco de dados


    }
}
