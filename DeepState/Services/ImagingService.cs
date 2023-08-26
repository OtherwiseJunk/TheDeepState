using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Services
{
    public class ImagingService
    {
        public static Image OverlayImage(Image bottomLayer, Image overlayLayer)
        {
            Image img = new Bitmap(bottomLayer.Width, bottomLayer.Height);
            using (Graphics gr = Graphics.FromImage(img))
            {
                gr.DrawImage(bottomLayer, new Point(0, 0));
                gr.DrawImage(overlayLayer, new Point(35, 35));
            }

            return img;
        }
    }
}
