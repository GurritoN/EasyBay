using Microsoft.EntityFrameworkCore;
using Storage;

namespace EasyBay.DataBase
{
    public class AuctionContext : DbContext
    {
        public AuctionContext(DbContextOptions<AuctionContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lot>().HasKey(x => x.Id);
            modelBuilder.Entity<Lot>().HasOne(x => x.CurrentBuyer);
            modelBuilder.Entity<Lot>().HasOne(x => x.Owner).WithMany(x => x.LotsForSale);
            modelBuilder.Entity<Lot>().HasMany(x => x.Tags);

            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().HasMany(x => x.BoughtLots);
            modelBuilder.Entity<User>().HasMany(x => x.TrackedLots);
            modelBuilder.Entity<User>().HasMany(x => x.LotsForSale).WithOne(x => x.Owner);

            modelBuilder.Entity<Tag>().HasKey(x => x.Id);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
