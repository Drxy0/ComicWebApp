using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace ComicWebApp.API.Features.ComicSeries;

public static class WebpConverter
{
    public static async Task<MemoryStream>? ConvertToWebpAsync(string imagePath, int quality, float resolution = 1.0f)
    {

        if (quality < 1 || quality > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(quality), "Quality must be between 1 - 100");
        }

        if (resolution < 0.0f || resolution > 1.0f)
        {
            throw new ArgumentOutOfRangeException(nameof(quality), "Resolution must be between 0.0 - 1.0");
        }

        MemoryStream memoryStream = new MemoryStream();

        try
        {
            using (Image image = await Image.LoadAsync(imagePath))
            {
                if (resolution != 1.0f)
                {
                    image.Mutate(x => x.Resize((int)(image.Width * resolution), (int)(image.Height * resolution)));
                }

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
