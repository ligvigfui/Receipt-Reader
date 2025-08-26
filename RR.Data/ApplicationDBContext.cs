namespace RR.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<UserDBO, RoleDBO, string, IdentityUserClaim<string>,
    UserRoleDBO, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>(options)
{
    public DbSet<GroupDBO> Groups { get; set; }
    public DbSet<UserGroupDBO> UserGroups { get; set; }
    public DbSet<AddressDBO> Addresses { get; set; }
    public DbSet<VendorHQDBO> VendorHQs { get; set; }
    public DbSet<VendorDBO> Vendors { get; set; }
    public DbSet<ReceiptDBO> Receipts { get; set; }
    public DbSet<ImageDBO> Images { get; set; }
    public DbSet<ReceiptItemDBO> ReceiptItems { get; set; }
    public DbSet<ProductDBO> Products { get; set; }
    public DbSet<ProductCategoryDBO> ProductCategories { get; set; }
    public DbSet<CategoryDBO> Categories { get; set; }

    [DbFunction("Levenshtein", "dbo")]
    public static int Levenshtein(string s1, string s2) => throw new NotImplementedException();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map Levenshtein function for use in LINQ queries
        modelBuilder.HasDbFunction(typeof(ApplicationDbContext).GetMethod(nameof(Levenshtein), [typeof(string), typeof(string)])!)
            .HasName("Levenshtein")
            .HasSchema("dbo");

        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        modelBuilder.Entity<RoleDBO>().ToTable(TablesAttribute.Pluralize(nameof(RoleDBO)));
        modelBuilder.Entity<UserRoleDBO>().ToTable(TablesAttribute.Pluralize(nameof(UserRoleDBO)));
        modelBuilder.Entity<UserDBO>().ToTable(TablesAttribute.Pluralize(nameof(UserDBO)));

        modelBuilder.Entity<RoleDBO>(b =>
        {
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        });

        modelBuilder.Entity<UserDBO>(u =>
        {
            u.HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            u.HasMany(u => u.Receipts)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.SetNull);
            u.HasMany(u => u.Images)
                .WithOne(i => i.User)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<GroupDBO>(g =>
        {
            g.HasMany(g => g.Users)
                .WithMany(u => u.Groups)
                .UsingEntity<UserGroupDBO>(
                    j => j
                        .HasOne(ug => ug.User)
                        .WithMany(u => u.UserGroups)
                        .HasForeignKey(ug => ug.UserId),
                    j => j
                        .HasOne(ug => ug.Group)
                        .WithMany(g => g.UserGroups)
                        .HasForeignKey(ug => ug.GroupId)
                );
            g.HasMany(g => g.Receipts)
                .WithOne(i => i.Group)
                .HasForeignKey(i => i.GroupId)
                .OnDelete(DeleteBehavior.SetNull);
            g.HasMany(g => g.Images)
                .WithOne(i => i.Group)
                .HasForeignKey(i => i.GroupId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<VendorHQDBO>(hq =>
        {
            hq.HasOne(hq => hq.Address)
                .WithMany()
                .HasForeignKey(hq => hq.AddressId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<VendorDBO>(v =>
        {
            v.HasOne(v => v.Address)
                .WithMany()
                .HasForeignKey(v => v.AddressId)
                .OnDelete(DeleteBehavior.SetNull);
            v.HasOne(v => v.HQ)
                .WithMany(hq => hq.Vendors)
                .HasForeignKey(v => v.HQId)
                .OnDelete(DeleteBehavior.SetNull);
        });
        var exclusiveOwnershipConstraint = $"({nameof(ReceiptDBO.UserId)} IS NOT NULL AND {nameof(ReceiptDBO.GroupId)} IS NULL) OR ({nameof(ReceiptDBO.UserId)} IS NULL AND {nameof(ReceiptDBO.GroupId)} IS NOT NULL)";
        modelBuilder.Entity<ReceiptDBO>(r =>
        {
            r.HasOne(r => r.Vendor)
                .WithMany(v => v.Receipts)
                .HasForeignKey(r => r.VendorId)
                .IsRequired();
            r.HasOne(r => r.Image)
                .WithMany()
                .HasForeignKey(r => r.ImageId)
                .OnDelete(DeleteBehavior.SetNull);
            r.HasMany(r => r.Items)
                .WithOne(ri => ri.Receipt)
                .HasForeignKey(ri => ri.ReceiptId)
                .IsRequired();
            r.ToTable(r => r.HasCheckConstraint("CK_Owner", exclusiveOwnershipConstraint));
        });

        modelBuilder.Entity<ImageDBO>(i =>
        {
            i.ToTable(i => i.HasCheckConstraint("CK_Owner", exclusiveOwnershipConstraint));
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
                    .HasForeignKey(pc => pc.ProductId)
            );

        modelBuilder.Entity<CategoryDBO>()
            .HasMany(c => c.SubCategories)
            .WithOne(c => c.ParentCategory)
            .HasForeignKey(c => c.ParentCategoryId);
    }
}
