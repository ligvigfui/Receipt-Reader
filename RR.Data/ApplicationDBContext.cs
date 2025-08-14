namespace RR.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<UserDBO, RoleDBO, string, IdentityUserClaim<string>,
    UserRoleDBO, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>(options)
{
    public DbSet<AddressDBO> Addresses { get; set; }
    public DbSet<ReceiptDBO> Receipts { get; set; }
    public DbSet<ReceiptItemDBO> ReceiptItems { get; set; }
    public DbSet<VendorDBO> Vendors { get; set; }
    public DbSet<VendorHQDBO> VendorHQs { get; set; }
    public DbSet<CategoryDBO> Categories { get; set; }
    public DbSet<CategoryPropertyDBO> CategoryProperties { get; set; }
    public DbSet<PropertyEnumValueDBO> PropertyEnumValues { get; set; }
    public DbSet<ItemCategoryDBO> ItemCategories { get; set; }
    public DbSet<CategoryPropertyValueDBO> CategoryPropertyValues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaimDBO");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaimDBO");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLoginDBO");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokenDBO");
        modelBuilder.Entity<UserDBO>().ToTable(nameof(UserDBO));
        modelBuilder.Entity<RoleDBO>().ToTable(nameof(RoleDBO));
        modelBuilder.Entity<UserRoleDBO>().ToTable(nameof(UserRoleDBO));

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
        // Composite keys and relationships for new tables
        modelBuilder.Entity<ItemCategoryDBO>()
            .HasKey(ic => new { ic.ItemId, ic.CategoryId });
        modelBuilder.Entity<ItemCategoryDBO>()
            .HasOne(ic => ic.Item)
            .WithMany(i => i.ItemCategories)
            .HasForeignKey(ic => ic.ItemId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<ItemCategoryDBO>()
            .HasOne(ic => ic.Category)
            .WithMany(c => c.ItemCategories)
            .HasForeignKey(ic => ic.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CategoryPropertyValueDBO>()
            .HasKey(cp => new { cp.CategoryId, cp.EnumValueId });
        modelBuilder.Entity<CategoryPropertyValueDBO>()
            .HasOne(cp => cp.Category)
            .WithMany(c => c.CategoryPropertyValues)
            .HasForeignKey(cp => cp.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<CategoryPropertyValueDBO>()
            .HasOne(cp => cp.PropertyEnumValue)
            .WithMany(ev => ev.CategoryPropertyValues)
            .HasForeignKey(cp => cp.EnumValueId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PropertyEnumValueDBO>()
            .HasIndex(ev => new { ev.PropertyId, ev.EnumValue })
            .IsUnique();
    }
}
