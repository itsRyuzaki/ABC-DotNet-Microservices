namespace ABC.Accessories.Services.Blob;

public interface IBlobService
{
    public Task<bool> Upload(string containerName, string path, IFormFile image);

}