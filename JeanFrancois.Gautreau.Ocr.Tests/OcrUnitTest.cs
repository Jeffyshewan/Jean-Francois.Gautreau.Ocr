using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks; 
using Xunit;

namespace JeanFrancois.Gautreau.Ocr.Tests;

public class OcrUnitTest
{
    
    [Fact]
    public async Task ImagesShouldBeReadCorrectly()
    {
        var executingPath = GetExecutingPath();
        var images = new List<byte[]>();
        foreach (var imagePath in Directory.EnumerateFiles(Path.Combine(executingPath, "Images")))
        {
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            images.Add(imageBytes);
        }

        var ocrResults = await new Ocr().ReadTextInImage(images);
        
        Assert.Equal(ocrResults[0].Text,"roc\n\nL'investissement en termes d'effort n'étant pas identique " +
                                        "sur ces\n\naspects, cela ne veut pas dire que l'investissement sur d'autres sera\n\n" +
                                        "différent également. Par exemple les capacités de diagnostic doi\n\n7\n");
        Assert.Equal(ocrResults[0].Confidence, 0.8899999856948853);
        
        Assert.Equal(ocrResults[1].Text, "différent également. Par exemple les capacités de diagnostic " +
                                         "doi\nvent être les mêmes sur .NET 5, tant pour les diagnostics fonction-\n\nnels " +
                                         "que pour les diagnostics de performance.\n");
        Assert.Equal(ocrResults[1].Confidence, 0.9399999976158142);
        
        Assert.Equal(ocrResults[2].Text, "Toutes les applications .NET 5 seront également constructibles\n\n" +
                                         "avec le .NET CLI, en assurant aux développeurs de pouvoir disposer\ndes mêmes " +
                                         "outils en ligne de commande. Enfin C# évoluera sur\n");
        Assert.Equal(ocrResults[2].Confidence, 0.9300000071525574);
    }
    private static string GetExecutingPath()
    {
        var executingAssemblyPath =
            Assembly.GetExecutingAssembly().Location;
        var executingPath =
            Path.GetDirectoryName(executingAssemblyPath);
        return executingPath;
    }
}