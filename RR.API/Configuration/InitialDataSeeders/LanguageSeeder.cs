namespace RR.API.Configuration.InitialDataSeeders;

public class LanguageSeeder(
    ApplicationDbContext context
) : ISeedable
{
    public async Task<bool> ShouldSeed() => !await context.Languages.AnyAsync();
    public Task Seed()
    {
        var english = new LanguageDBO()
        {
            LanguageCode = "en-US",
            LanguageName = "English",
            CultureName = "US"
        };
        var hungarian = new LanguageDBO()
        {
            LanguageCode = "hu-HU",
            LanguageName = "Hungarian",
            CultureName = "HU"
        };
        context.Languages.AddRange(english, hungarian);
        return context.SaveChangesAsync();
    }

}
