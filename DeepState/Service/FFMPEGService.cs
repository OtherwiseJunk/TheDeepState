using Discord;
using Discord.Commands;
using FFMpegCore.Extend;
using System;
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

        public async Task<bool> AddWilhelmToAttachmentAndSend(string attachmentUrl, MediaType mediaType, ICommandContext Context, string filename = "output")
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, attachmentUrl))
            {
                try
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
                                AddWilhemToImage(response.Content.ReadAsStream(), Context, filename);
                                return true;
                            case MediaType.Webp:
                                return false;
                            default:
                                return false;
                        }
                    }
                }
                catch(Exception ex)
                {
                    _ = Context.Message.ReplyAsync("Something went horribly wrong when I tried to get that media. Dunno why.");
                    return false;
                }
            }
        }

        private void AddWilhemToImage(Stream fileStream, ICommandContext Context, string fileName)
        {
            System.Drawing.Image image = MakeImageDimensionsEven(System.Drawing.Image.FromStream(fileStream));
            try
            {
                image.AddAudio("./wilhelm.ogg", $"{fileName}.mp4");
                
                _ = Context.Channel.SendFileAsync($"./{fileName}.mp4", messageReference: new MessageReference(Context.Message.Id));
            }
            catch(Exception ex){
                Console.WriteLine($"Encountered an exception: {ex.Message}");
                _ = Context.Channel.SendMessageAsync("Sorry, either the filetype of the attachment is not supported at this time, but it had", messageReference: Context.Message.Reference);
            }
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
