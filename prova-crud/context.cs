using Microsoft.EntityFrameworkCore;
using System.IO;

public class ImovelContext : DbContext
{
    public DbSet<Imovel> Imoveis { get; set; }
    public DbSet<Comodo> Comodos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={Path.Combine(Directory.GetCurrentDirectory(), "imoveis.db")}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Imovel>()
            .HasMany(i => i.Comodos)
            .WithOne(c => c.Imovel)
            .HasForeignKey(c => c.ImovelId);
    }
}