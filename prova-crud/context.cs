using Microsoft.EntityFrameworkCore;

public class ImovelContext : DbContext
{
    public ImovelContext(DbContextOptions<ImovelContext> options) : base(options) { }

    public DbSet<Imovel> Imoveis { get; set; }
    public DbSet<Comodo> Comodos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Imovel>()
            .HasMany(i => i.Comodos)
            .WithOne(c => c.Imovel)
            .HasForeignKey(c => c.ImovelId);

        modelBuilder.Entity<Imovel>().ToTable("Imoveis");
        modelBuilder.Entity<Comodo>().ToTable("Comodos");
    }
}