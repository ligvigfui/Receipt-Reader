namespace RR.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<UserDBO, RoleDBO, string, IdentityUserClaim<string>,
    UserRoleDBO, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>(options)
{
    public DbSet<AddressDBO> Addresses { get; set; }
    public DbSet<ReceiptDBO> Receipts { get; set; }
    public DbSet<VendorDBO> Vendors { get; set; }
    public DbSet<VendorHQDBO> VendorHQs { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaimDBO");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaimDBO");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLoginDBO");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokenDBO");

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
    }
}
