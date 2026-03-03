using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Domain.Entities;
using PortfolioTracker.Infrastructure.Identity;

namespace PortfolioTracker.Infrastructure.Persistence;

internal class PortfolioTrackerDbContext(DbContextOptions<PortfolioTrackerDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    internal DbSet<Portfolio> Portfolios { get; set; }
    internal DbSet<PortfolioItem> PortfolioItems { get; set; }
    internal DbSet<PositionComment> PositionComments { get; set; }
    internal DbSet<Signal> Signals { get; set; }
    internal DbSet<SignalRule> SignalRules { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Portfolio>()
            .HasMany(p => p.Items)
            .WithOne()
            .HasForeignKey(pi => pi.PortfolioId);

        builder.Entity<PortfolioItem>()
            .HasMany(pi => pi.Comments)
            .WithOne()
            .HasForeignKey(c => c.PortfolioItemId);
    }

}
