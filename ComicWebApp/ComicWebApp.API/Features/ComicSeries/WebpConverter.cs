using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

namespace ComicWebApp.API.Features.ComicSeries;

public static class WebpConverter
{
    public static async Task<MemoryStream>? ConvertToWebpAsync(string imagePath, int quality)
    {

        if (quality < 1 || quality > 100)
        {
            // QUESTION: How to handle this case
            throw new ArgumentOutOfRangeException(nameof(quality), "Quality must be between 1-100");
        }

        using (MemoryStream memoryStream = new MemoryStream())
        {
            try
            {
                using (Image image = await Image.LoadAsync(imagePath))
                {
                    WebpEncoder encoder = new WebpEncoder
                    {
                        Quality = quality, // 70 is good balance
                        Method = WebpEncodingMethod.Default,
                        UseAlphaCompression = true,
                        SkipMetadata = true
                    };

                    await image.SaveAsync(memoryStream, encoder);
                }

                memoryStream.Position = 0;
                return memoryStream;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to process image: {ex.Message}");
                return null!;
            }
        }
        
    }
}
