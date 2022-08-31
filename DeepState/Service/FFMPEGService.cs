using Discord;
using FFMpegCore.Extend;
using System.Collections.Generic;

namespace DeepState.Service
{
    public class FFMPEGService
    {
        public string ImageURLRegex = @"(http(s?):)([/|.|\w|\s|-])*\.(?:jpg|gif|png|webp)";
        public IAttachment? AttachmentHasSingularAcceptableMediaType(List<IAttachment> attachments, out MediaType? attachmentMediaType)
        {
            if (attachments.Count == 0 || attachments.Count > 1)
            {
                attachmentMediaType = null;
                return null;
            }
            attachmentMediaType = null;
            return null;
        }

        public System.Drawing.Image AddWilhemToImage(System.Drawing.Image image, IAttachment attachments, string fileName = "output")
        {
            image.AddAudio("./wilhelm.ogg", "output.mp4");
            
        } 
    }

    public enum MediaType
    {
        Image,
        AnimatedImage,
        Video
    };
}
