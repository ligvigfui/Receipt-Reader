namespace RR.Data.Repository;

public class ImageRepository (
    ApplicationDbContext context
) : IImageRepository
{
    public async Task<ImageDBO> CreateImageAsync(
        ImageDBO imageDBO
    )
    {
        await context.Images.AddAsync(imageDBO);
        await context.SaveChangesAsync();
        return imageDBO;
    }
    public async Task<string?> GetImageBlobUrlAsync(string fileName, string userId, int? groupId)
    {
        return await context.Images
            .Where(i => i.FileName == fileName && i.UserId == userId && i.GroupId == groupId)
            .Select(i => i.BlobGuid)
            .FirstOrDefaultAsync();
    }
}