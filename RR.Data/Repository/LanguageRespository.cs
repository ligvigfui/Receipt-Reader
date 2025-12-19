namespace RR.Data.Repository;

public class LanguageRespository(
    ApplicationDbContext context
) : ILanguageRespository
{
    public async Task<LanguageDBO?> TryGetLanguageByCodeAsync(string languageCode) =>
        await context.Languages
            .Where(l => l.LanguageCode == languageCode)
            .FirstOrDefaultAsync();
    public async Task<LanguageDBO> GetLanguageByCodeAsync(string languageCode) =>
        await context.Languages
            .Where(l => l.LanguageCode == languageCode)
            .FirstOrDefaultAsync() ??
            throw new NotFoundException($"Language with code '{languageCode}' not found.");
    public async Task<LanguageDBO> GetLanguageForUserAsync(string? languageCode, UserDBO user) =>
        await context.Languages
            .Where(l => l.LanguageCode == languageCode)
            .FirstOrDefaultAsync() ??
            await context.Languages
                .Where(l => l.Id == user.DefaultLanguageId)
                .FirstOrDefaultAsync() ??
                throw new NotFoundException("No valid language found.");

    public async Task<LanguageDBO> CreateLanguageAsync(LanguageDBO language)
    {
        var existingLanguage = await context.Languages
            .Where(l => l.LanguageCode == language.LanguageCode)
            .FirstOrDefaultAsync();
        if (existingLanguage is not null)
            throw new InvalidOperationException($"Language with code '{language.LanguageCode}' already exists.");
        context.Languages.Add(language);
        await context.SaveChangesAsync();
        return language;
    }
}
