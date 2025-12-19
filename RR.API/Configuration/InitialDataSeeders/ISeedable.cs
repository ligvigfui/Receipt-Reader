namespace RR.API.Configuration.InitialDataSeeders;

public interface ISeedable
{
    Task<bool> ShouldSeed();
    Task Seed();
}