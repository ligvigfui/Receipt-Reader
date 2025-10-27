namespace RR.Settings;

public class AzureBlobStorageSettings
{
    public string BlobServiceEndpoint { get; set; }
    public string ConnectionString { get; set; }
    public string PublicContainerName { get; set; }
    public string PrivateContainerName { get; set; }
}
