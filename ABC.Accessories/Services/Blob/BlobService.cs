namespace ABC.Accessories.Services.Blob;

using ABC.Accessories.DTO.Response;
using ABC.Accessories.Enums;
using Azure.Storage.Blobs;

public class BlobService : IBlobService
{
    private readonly BlobServiceClient _serviceClient;
    private readonly ILogger<BlobService> _logger;

    public BlobService(ILogger<BlobService> logger)
    {
        _logger = logger;

        // Connection string for Azurite Blob Storage
        // string connectionString = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOxKkxp8tV7sKlq8bfa8f3FqFtMgyQ87OmG0Bsz43WfImdDkYZp2P4HNhqcuWzO5z5OisXB2ZaLw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;";

        string connectionString = "UseDevelopmentStorage=true";

        // Create BlobServiceClient
        _serviceClient = new(connectionString);


    }

    public async Task<ApiResponseDto<string>> Upload(string containerName, string path, IFormFile file)
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
            return ApiResponseDto.HandleSuccessResponse("File uploaded successfully to blob");

        }

        catch (Exception error)
        {
            _logger.LogError("Error occured while uploading file to blob with path: {path}. See error stack below: {error}", path, error);
            return ApiResponseDto.HandleErrorResponse((int)ResponseCode.ERROR, ["Error occurred while uploading file to blob"]);
        }

    }

}