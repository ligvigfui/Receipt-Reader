namespace RR.Data.Interfaces;

public interface ILanguageRespository
{
    Task<LanguageDBO> CreateLanguageAsync(LanguageDBO language);
    Task<LanguageDBO> GetLanguageByCodeAsync(string languageCode);
    Task<LanguageDBO> GetLanguageForUserAsync(string? languageCode, UserDBO user);
    Task<LanguageDBO?> TryGetLanguageByCodeAsync(string languageCode);
}
