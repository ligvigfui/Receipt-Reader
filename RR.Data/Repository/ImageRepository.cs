namespace RR.Data.Repository;

public class ImageRepository (
    ApplicationDbContext context
) : IImageRepository
{
    public async Task<ImageDBO> CreateImageAsync(ImageDBO imageDBO)
    {
        await context.Images.AddAsync(imageDBO);
        await context.SaveChangesAsync();
        return imageDBO;
    }
    public async Task<ImageDBO?> GetImageBlobUrlAsync(string fileName, int userShortId)
    {
        return await context.Images
            .WhereCanRead(userShortId)
            .Where(i => i.FileName == fileName)
            .FirstOrDefaultAsync();
    }
}