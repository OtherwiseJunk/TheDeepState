using DartsDiscordBots.Services;
using DeepState.Constants.CharacterGeneration;
using DeepState.Data.Constants;
using Svg;
using System;
using System.IO;
using System.Net;

namespace DeepState.Service
{
    public class RandomCharacterImageService
    {
        ImagingService _imagingService;

        public RandomCharacterImageService(ImagingService imagingService)
        {
            _imagingService = imagingService;
        }

        public string CreateAndUploadCharacterImage(string avatarFlavor = AvatarConstants.Avataaars)
        {
            using (WebClient wc = new WebClient())
            {
                string guidId = Guid.NewGuid().ToString();
                string svgFile = $"{guidId}.svg";
                wc.DownloadFile($"https://api.dicebear.com/6.x/{avatarFlavor}/svg?seed={guidId}", svgFile);
                var svgDoc = SvgDocument.Open<SvgDocument>(svgFile, null);
                Stream stream = new MemoryStream();
                svgDoc.Draw().Save(stream, System.Drawing.Imaging.ImageFormat.Png);


                return _imagingService.UploadImage(RPGConstants.AvatarFolder, stream);

            }
        }
    }
}