using Discord;
using FFMpegCore.Extend;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeepState.Service
{
    public class FFMPEGService
    {
        public string ImageURLRegex = @"(http(s?):)([/|.|\w|\s|-])*\.(?:jpg|png)";
        public string VideoUrlRegex = @"(http(s?):)([/|.|\w|\s|-])*\.mp4";
        public string AnimateImageURLRegex = @"(http(s?):)([/|.|\w|\s|-])*\.(?:gif|gifv)";
        public string WebpUrlRegex = @"(http(s?):)([/|.|\w|\s|-])*\.webp";
        public string MediaUrlRegex = @"(http(s?):)([/|.|\w|\s|-])*\.(?:gif|gifv|webp|mp4|jpg|png)";
        public HttpClient _httpClient { get; set; }

        public FFMPEGService(HttpClient client)
        {
            _httpClient = client;
        }

        public string GetSingleAcceptableAttachmentUrl(List<IAttachment> attachments, out MediaType? attachmentMediaType)
        {
            attachmentMediaType = null;
            if (attachments.Count == 0 || attachments.Count > 1)
            {
                return null;
            }
            IAttachment attachment = attachments[0];
            attachmentMediaType = GetMediaTypeFromUrl(attachment.Url);
            return attachment.Url;
        }

        public string GetSingleAcceptableContentUrl(string messageContent, out MediaType? contentMediaType)
        {
            contentMediaType = null;
            string url = null;
            Match match = Regex.Match(messageContent, MediaUrlRegex);
            if(match.Success && match.Captures.Count == 1)
            {
                url = match.Captures[0].Value;
            }
            if (url != null)
            {
                contentMediaType = GetMediaTypeFromUrl(url);
            }
            return url;
        }

        public MediaType? GetMediaTypeFromUrl(string url)
        {
            MediaType? urlMediaType = null;
            if (Regex.Match(url, VideoUrlRegex).Success)
            {
                urlMediaType = MediaType.Video;
            }
            if (Regex.Match(url, AnimateImageURLRegex).Success)
            {
                urlMediaType = MediaType.AnimatedImage;
            }
            if (Regex.Match(url, WebpUrlRegex).Success)
            {
                urlMediaType = MediaType.Webp;
            }
            if (Regex.Match(url, ImageURLRegex).Success)
            {
                urlMediaType = MediaType.Image;
            }
            return urlMediaType;
        }

        public async Task<bool> AddWilhelmToAttachment(string attachmentUrl, MediaType mediaType, string filename = "output")
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, attachmentUrl))
            {
                using (HttpResponseMessage response = await _httpClient.SendAsync(request))
                {
                    switch (mediaType)
                    {
                        case MediaType.Video:
                            return false;
                        case MediaType.AnimatedImage:
                            return false;
                        case MediaType.Image:
                            return AddWilhemToImage(response.Content.ReadAsStream(), filename);
                        case MediaType.Webp:
                            return false;
                        default:
                            return false;
                    }
                }
            }
        }

        private bool AddWilhemToImage(Stream fileStream, string fileName)
        {
            System.Drawing.Image image = MakeImageDimensionsEven(System.Drawing.Image.FromStream(fileStream));
            return image.AddAudio("./wilhelm.ogg", $"{fileName}.mp4");            
        }

        private System.Drawing.Image MakeImageDimensionsEven(System.Drawing.Image image)
        {
            int height = image.Height % 2 == 0 ? image.Height : image.Height + 1;
            int width = image.Width % 2 == 0 ? image.Width : image.Width + 1;
            if (image.Height % 2 != 0 || image.Width % 2 != 0)
            {
                image = new Bitmap(image, new Size(width, height));
            }
            return image;
        }

    }


    public enum MediaType
    {
        Image,
        AnimatedImage,
        Webp,
        Video
    };
}
