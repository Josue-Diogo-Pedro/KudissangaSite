using KudissangaSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KudissangaSite.Areas.Identity.Data;

public class KudissangaSiteContext : IdentityDbContext<IdentityUser>
{
    public KudissangaSiteContext(DbContextOptions<KudissangaSiteContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<Pessoa> Pessoas { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Notificacao> Notificacoes { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<ReservaItem> ReservaItems { get; set; }
    public DbSet<Suite> Suites { get; set; }
}
