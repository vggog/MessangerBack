using Microsoft.EntityFrameworkCore;
using MessangerBack.Models;


namespace MessangerBack.DataBase;

public class DataBaseContext : DbContext
{
    public DbSet<UserModel> Users { get; set; }

    public DataBaseContext (DbContextOptions<DataBaseContext> options) : base(options) 
    { 
        Database.EnsureCreated();
    }

    // public DataBaseContext()
    // {
    //     Database.EnsureCreated();
    // }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=usersdb;Username=postgres;Password=здесь_указывается_пароль_от_postgres");
    // }
}
