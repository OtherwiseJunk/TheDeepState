using DartsDiscordBots.Services;
using DeepState.Data.Constants;
using Svg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Service
{
    public class RandomCharacterImageService
    {
        ImagingService _imagingService;

        public RandomCharacterImageService(ImagingService imagingService)
        {
            _imagingService = imagingService;
        }

        public string CreateAndUploadCharacterImage()
        {
            using (WebClient wc = new WebClient())
            {
                string guidId = Guid.NewGuid().ToString();
                string svgFile = $"{guidId}.svg";
                wc.DownloadFile($"https://avatars.dicebear.com/api/avataaars/{guidId}.svg", svgFile);
                var svgDoc = SvgDocument.Open<SvgDocument>(svgFile, null);
                Stream stream = new MemoryStream();
                svgDoc.Draw().Save(stream, System.Drawing.Imaging.ImageFormat.Png);


                return _imagingService.UploadImage(RPGConstants.AvatarFolder, stream);

            }
        }
    }
}