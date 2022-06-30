using Microsoft.AspNetCore.Mvc;
using PowerReact.Services;

namespace PowerReact.Controllers;

public class ImageController :BaseApiController
{
    private readonly ImageService _imageService;

    public ImageController(ImageService imageService)
    {
        _imageService = imageService;
    }


    [HttpGet]
    [Route("/Images/{imageName}")]
    public async Task<ActionResult<byte[]>> GetImage(string imageName)
    {
        var tuple = await _imageService.GetImage(imageName);
        var fileBytes =  tuple.Item1;
        var fileType = tuple.Item2;
        return File(fileBytes,fileType);
    }
}