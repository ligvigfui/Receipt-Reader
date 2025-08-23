namespace RR.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<UserDBO, RoleDBO, string, IdentityUserClaim<string>,
    UserRoleDBO, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>(options)
{
    public DbSet<AddressDBO> Addresses { get; set; }
    public DbSet<VendorDBO> Vendors { get; set; }
    public DbSet<VendorHQDBO> VendorHQs { get; set; }
    public DbSet<ReceiptDBO> Receipts { get; set; }
    public DbSet<ReceiptItemDBO> ReceiptItems { get; set; }
    public DbSet<ProductDBO> Products { get; set; }
    public DbSet<ProductCategoryDBO> ProductCategories { get; set; }
    public DbSet<CategoryDBO> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        modelBuilder.Entity<UserDBO>().ToTable(TablesAttribute.Pluralize(nameof(UserDBO)));
        modelBuilder.Entity<RoleDBO>().ToTable(TablesAttribute.Pluralize(nameof(RoleDBO)));
        modelBuilder.Entity<UserRoleDBO>().ToTable(TablesAttribute.Pluralize(nameof(UserRoleDBO)));
        modelBuilder.Entity<UserDBO>(u =>
        {
            u.HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });

        modelBuilder.Entity<RoleDBO>(b =>
        {
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        });

        // Iterate over all entity types in the model
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Iterate over all foreign keys in the entity type
            foreach (var foreignKey in entityType.GetForeignKeys())
            {
                // Set the delete behavior to Restrict
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
        modelBuilder.Entity<ReceiptItemDBO>()
            .HasOne(ri => ri.Product)
            .WithMany(p => p.ReceiptItems)
            .HasForeignKey(ri => ri.ProductId);
        modelBuilder.Entity<ProductDBO>()
            .HasMany(p => p.Categories)
            .WithMany(c => c.Products)
            .UsingEntity<ProductCategoryDBO>(
                j => j
                    .HasOne(pc => pc.Category)
                    .WithMany(c => c.ProductCategories)
                    .HasForeignKey(pc => pc.CategoryId),
                j => j
                    .HasOne(pc => pc.Product)
                    .WithMany(p => p.ProductCategories)
                    .HasForeignKey(pc => pc.ProductId),
                j =>
                {
                    j.HasKey(t => new { t.ProductId, t.CategoryId });
                });
        modelBuilder.Entity<CategoryDBO>()
            .HasMany(c => c.SubCategories)
            .WithOne(c => c.ParentCategory)
            .HasForeignKey(c => c.ParentCategoryId);
    }
}
