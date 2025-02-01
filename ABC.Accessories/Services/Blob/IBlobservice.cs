using ABC.Accessories.DTO.Response;

namespace ABC.Accessories.Services.Blob;

public interface IBlobService
{
    public Task<ApiResponseDto<string>> Upload(string containerName, string path, IFormFile image);

}