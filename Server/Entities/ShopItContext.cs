using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Server.Entities
{
    public class ShopItContext :DbContext
    {
        public ShopItContext(DbContextOptions<ShopItContext> options) : base(options) { }

        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Review>(entity =>
                {
                    entity
                    .Property(r => r.Score)
                    .IsRequired();
                });

            modelBuilder
                .Entity<Book>()
                .HasMany(r=>r.Review)
                .WithOne(r => r.Book)
                .HasForeignKey(r => r.BookId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); //if we delete book,delete all reviews

            modelBuilder
                .Entity<Book>()
                .Property(b => b.ReleaseDate)
                .HasColumnType("date") // Store only date part
                .IsRequired();
        }
    }
}
