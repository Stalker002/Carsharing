using Microsoft.AspNetCore.Http;

namespace Carsharing.Application.DTOs;

public class UploadImageDto
{
    public IFormFile Image { get; set; }
}