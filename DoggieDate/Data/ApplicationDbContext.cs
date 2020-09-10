using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DoggieDate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DoggieDate.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            // Ignore IdentityUser properties
            builder.Entity<ApplicationUser>().Ignore(c => c.AccessFailedCount)
                                             .Ignore(c => c.LockoutEnabled)
                                             .Ignore(c => c.LockoutEnd)
                                             .Ignore(c => c.PhoneNumber)
                                             .Ignore(c => c.PhoneNumberConfirmed)
                                             .Ignore(c => c.TwoFactorEnabled);

            // Set table name
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<Contact>().ToTable("Contacts");
            builder.Entity<Animal>().ToTable("Animals");
            builder.Entity<Message>().ToTable("Messages");

            // PK Contacts
            builder.Entity<Contact>().HasKey(x => new { x.UserId, x.ContactId });

            // User Contacts
            builder.Entity<Contact>()
                .HasOne(u => u.User)
                .WithMany(c => c.Contacts)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Contacts of users
            builder.Entity<Contact>()
                .HasOne(u => u.UserContact)
                .WithMany()
                .HasForeignKey(k => k.ContactId)
                .OnDelete(DeleteBehavior.NoAction);

            // Messages
            builder.Entity<Message>()
                .HasOne(u => u.Receiver)
                .WithMany(m => m.Messages)
                .HasForeignKey(k => k.ReceiverId);

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany()
                .HasForeignKey(k => k.SenderId);
        }

        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<Contact> Contact { get; set; }
    }
}
