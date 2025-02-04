namespace ABC.Accessories.Services.Blob;

using Azure.Storage.Blobs;

public class BlobService : IBlobService
{
    private readonly BlobServiceClient _serviceClient;
    private readonly ILogger<BlobService> _logger;

    public BlobService(ILogger<BlobService> logger, IConfiguration configuration)
    {
        _logger = logger;

        string? connectionString = configuration.GetConnectionString("ABC_Blob");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new Exception("No connection string provided for Blob Service");
        }
        else
        {
            // Create BlobServiceClient
            _serviceClient = new(connectionString);
        }


    }

    public async Task<bool> Upload(string containerName, string path, IFormFile file)
    {
        try
        {
            var containerClient = _serviceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            _logger.LogInformation("Connected to Azurite Blob Storage and checked container: {name}", containerName);

            var blobClient = containerClient.GetBlobClient(path);

            // Upload image
            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            _logger.LogInformation("File uploaded successfully to blob with path: {path}", path);
            return true;

        }
        catch (Exception error)
        {
            _logger.LogError("Error occured while uploading file to blob with path: {path}. See error stack below: {error}", path, error);
            return false;
        }

    }

}