using Core;
using Microsoft.EntityFrameworkCore;

public class ApgNotificationsDbContext : DbContext
{
    public ApgNotificationsDbContext(DbContextOptions<ApgNotificationsDbContext> options) : base(options)
    {
    }

    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationConfig> NotificationsConfig { get; set; }
    public DbSet<Agente> Agentes { get; set; }
    public DbSet<Suscripcion> Suscripciones { get; set; }
    public DbSet<AgenteSuscripcion> AgenteSuscripciones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AgenteSuscripcion>()
            .HasKey(a => new { a.AgenteId, a.SuscripcionId });

        modelBuilder.Entity<AgenteSuscripcion>()
            .HasOne(a => a.Agente)
            .WithMany(a => a.AgenteSuscripciones)
            .HasForeignKey(a => a.AgenteId);

        modelBuilder.Entity<AgenteSuscripcion>()
            .HasOne(a => a.Suscripcion)
            .WithMany(s => s.AgenteSuscripciones)
            .HasForeignKey(a => a.SuscripcionId);
    }
}
