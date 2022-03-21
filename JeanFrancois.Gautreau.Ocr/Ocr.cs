using System.Reflection;
using Tesseract;

namespace JeanFrancois.Gautreau.Ocr;

public class Ocr
{
    public async Task<IList<OcrResult>> ReadTextInImage(IList<byte[]> images)
    {
        IList<OcrResult> ocrResults = new List<OcrResult>();
        foreach (var image in images)
        {
            ocrResults.Add(await Task.Run(() => DetectTextInImage(image)));
        }

        return ocrResults;
    }

    private static string GetExecutingPath()
    {
        var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
        var executingPath = Path.GetDirectoryName(executingAssemblyPath);

        return executingPath;
    }

    public OcrResult DetectTextInImage(byte[] image)
    {
        var ocrResultList = new List<OcrResult>();
        var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
        var executingPath = Path.GetDirectoryName(executingAssemblyPath);

        using var engine = new TesseractEngine(Path.Combine(executingPath, @"tessdata"), "fra", EngineMode.Default);
        using var pix = Pix.LoadFromMemory(image);
        var test = engine.Process(pix);

        return new OcrResult()
        {
            Text = test.GetText(),
            Confidence = test.GetMeanConfidence()
        };
    }
}