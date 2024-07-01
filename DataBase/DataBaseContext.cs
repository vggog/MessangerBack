using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MessangerBack.Models;
using MessangerBack.Configurations;


namespace MessangerBack.DataBase;

public class DataBaseContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
{    
    public DbSet<ChatModel> Chats { get; set; }
    public DbSet<MessageModel> Messages { get; set; }

    public DataBaseContext (DbContextOptions<DataBaseContext> options) : base(options) 
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ChatConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
