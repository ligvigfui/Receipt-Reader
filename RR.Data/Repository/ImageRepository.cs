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
    public async Task<string?> GetImageBlobUrlAsync(string fileName, int userShortId, int? groupId)
    {
        return await context.Images
            .Where(i => i.FileName == fileName && i.UserShortId == userShortId && i.GroupId == groupId)
            .Select(i => i.BlobGuid)
            .FirstOrDefaultAsync();
    }
}