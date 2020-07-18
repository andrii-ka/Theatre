using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Theatre.TicketOffice.Models;

namespace Theatre.TicketOffice.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShowTime>()
                .HasOne(st => st.Show)
                .WithMany(s => s.ShowTimes)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.ShowTime)
                .WithMany(st => st.Bookings)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                    .Property(b => b.BookedAtUtc)
                    .HasDefaultValueSql("getutcdate()");

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Show> Shows { get; set; }
        public DbSet<ShowTime> ShowTimes { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
