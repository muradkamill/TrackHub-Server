using System.Reflection;
using Domain;
using Domain.Auth;
using Domain.Card;
using Domain.Category;
using Domain.Comment;
using Domain.Product;
using Domain.SubCategory;
using Domain.Suggestion;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext(DbContextOptions options) :DbContext(options)
{
    public DbSet<PersonEntity> People { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<SubCategoryEntity> SubCategories { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<CartEntity> Carts { get; set; }
    public DbSet<CartEntity> Comments { get; set; }
    public DbSet<SuggestionEntity> Suggestion { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<PersonEntity>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<CategoryEntity>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<ProductEntity>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<SubCategoryEntity>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<CartEntity>().HasQueryFilter(x => x.IsDelete == false);
        modelBuilder.Entity<CommentEntity>().HasQueryFilter(x => x.IsDelete);
        modelBuilder.Entity<SuggestionEntity>().HasQueryFilter(x => x.IsDelete);






        modelBuilder.Entity<CategoryEntity>()
            .HasMany(x => x.CategoryToSubCategory)
            .WithOne(x => x.Category)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ProductEntity>()
            .HasOne(x => x.ProductToSubCategory)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.SubCategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CartEntity>()
            .HasOne(x => x.Product)
            .WithMany(x => x.Cart)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<PersonEntity>()
            .HasMany(x =>x.CartEntity)
            .WithOne(x => x.Person)
            .HasForeignKey(x => x.PersonFin);

        modelBuilder.Entity<ProductEntity>()
            .HasOne(x => x.PersonEntity)
            .WithMany(x => x.ProductEntity)
            .HasForeignKey(x => x.OwnerFin)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<CommentEntity>()
            .HasOne(x => x.Product)
            .WithMany(x => x.Comment)
            .HasForeignKey(x=>x.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}