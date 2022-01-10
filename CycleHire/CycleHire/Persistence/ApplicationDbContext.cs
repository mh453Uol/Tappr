using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CycleHire.Core.Models;

namespace CycleHire.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Listing> Listings { get; set; }
        public DbSet<Accessory> Accessories { get; set; }
        public DbSet<ListingAccessory> ListingsAccessories { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<HostReview> HostReviews { get; set; }
        public DbSet<TenantReview> TenantReviews { get; set; }
        public DbSet<Node> Routes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Listing>()
                .HasOne(u => u.User)
                .WithMany(l => l.Listings)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ListingAccessory>()
                .HasKey(k => new { k.ListingId, k.AccessoryId });

            builder.Entity<ListingAccessory>()
                .HasOne(la => la.Accessory)
                .WithMany()
                .HasForeignKey(la => la.AccessoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ListingAccessory>()
                .HasOne(la => la.Listing)
                .WithMany(l => l.Accessories)
                .HasForeignKey(la => la.ListingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Listing>()
                .HasOne(l => l.Availability)
                .WithOne(a => a.Listing)
                .HasForeignKey<Availability>(a => a.ListingId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Listing>()
                .HasMany(l => l.Images)
                .WithOne(i => i.Listing)
                .HasForeignKey(i => i.ListingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Listing>()
                .Property(l => l.Price)
                .HasColumnType("money");

            builder.Entity<Booking>()
                .HasOne(b => b.Listing)
                .WithMany(l => l.Bookings)
                .HasForeignKey(b => b.ListingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Booking>()
                .HasOne(b => b.Owner)
                .WithMany()
                .HasForeignKey(b => b.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Booking>()
                .Property(b => b.PricePerDay)
                .HasColumnType("money");

            builder.Entity<Booking>()
                .Property(b => b.TotalPrice)
                .HasColumnType("money");

            builder.Entity<Booking>()
                .Property(b => b.SubTotal)
                .HasColumnType("money");

            builder.Entity<Booking>()
                .Property(b => b.StripeTransactionFees)
                .HasColumnType("money");

            builder.Entity<Booking>()
                .Property(b => b.OurServiceFees)
                .HasColumnType("money");

            //builder.Entity<HostReview>()
            //    .ToTable("Reviews")
            //    .HasDiscriminator<byte>("ReviewType")
            //    .HasValue<HostReview>(1)
            //    .HasValue<TenantReview>(2);

            //-- Host Review -- 
            builder.Entity<HostReview>()
                .HasOne(r => r.Booking)
                .WithOne(b => b.HostReview)
                .HasForeignKey<HostReview>(b => b.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<HostReview>()
                .HasOne(r => r.Listing)
                .WithMany()
                .HasForeignKey(b => b.ListingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<HostReview>()
                .HasOne(r => r.User)
                .WithMany(r => r.HostReviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //-- Tenant Review 

            builder.Entity<TenantReview>()
                .HasOne(r => r.Booking)
                .WithOne(b => b.TenantReview)
                .HasForeignKey<TenantReview>(b => b.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TenantReview>()
                .HasOne(r => r.Listing)
                .WithMany()
                .HasForeignKey(b => b.ListingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TenantReview>()
                .HasOne(r => r.User)
                .WithMany(r => r.TenantReviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Node>()
                .HasDiscriminator<string>("Type")
                .HasValue<FolderItem>("Folder")
                .HasValue<RouteItem>("Route");

            builder.Entity<Node>()
                .HasOne(n => n.User)
                .WithMany(u => u.Routes)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FolderItem>();
            builder.Entity<RouteItem>();

            base.OnModelCreating(builder);
        }
    }
}
