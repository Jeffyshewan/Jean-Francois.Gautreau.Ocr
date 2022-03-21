using Microsoft.AspNetCore.Mvc;

namespace JeanFrancois.Gautreau.Ocr.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class OcrController : ControllerBase
{
    private readonly Ocr _ocr;
    public OcrController(Ocr ocr)
    {
        _ocr = ocr;
    }

    [HttpPost]
    public async Task<IList<OcrResult>>
        OnPostUploadAsync([FromForm(Name = "files")] IList<IFormFile> files)
    {
        var images = new List<byte[]>();
        foreach (var formFile in files)
        {
            using var sourceStream = formFile.OpenReadStream();
            using var memoryStream = new MemoryStream();
            sourceStream.CopyTo(memoryStream);
            images.Add(memoryStream.ToArray());
        }

        var ocrResult = await _ocr.ReadTextInImage(images);
        // Your implementation code
        throw new NotImplementedException();
    }
}