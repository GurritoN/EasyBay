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

            modelBuilder.Entity<Transaction>().HasKey(x => x.Id);
            modelBuilder.Entity<Transaction>().HasOne(x => x.Lot);
            modelBuilder.Entity<Transaction>().HasOne(x => x.Customer);

            modelBuilder.Entity<Log>().HasKey(x => x.Id);
            modelBuilder.Entity<Log>().HasOne(x => x.Transaction);

            modelBuilder.Entity<LotTag>().HasKey(x => x.Id);

            modelBuilder.Entity<UserType>().HasKey(x => x.Id);

            modelBuilder.Entity<LotForSale>().HasKey(x => x.Id);

            modelBuilder.Entity<BoughtLot>().HasKey(x => x.Id);

            modelBuilder.Entity<TrackedLot>().HasKey(x => x.Id);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<LotTag> LotsTags { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<LotForSale> LotsForSale { get; set; }
        public DbSet<BoughtLot> BoughtLots { get; set; }
        public DbSet<TrackedLot> TrackedLots { get; set; }
    }
}
