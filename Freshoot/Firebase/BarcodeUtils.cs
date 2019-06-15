using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Freshoot.Firebase
{
    class BarcodeUtils
    {
        public static void showBarcode(string code, PictureBox picturebox)
        {
            var myBitmap = new Bitmap(250, 140);
            var g = Graphics.FromImage(myBitmap);
            var jgpEncoder = GetEncoder(ImageFormat.Jpeg);

            g.Clear(Color.White);

            var strFormat = new StringFormat { Alignment = StringAlignment.Center };
            g.DrawString(code, new Font("Free 3 of 9", 15), Brushes.Black, new RectangleF(0, 50, 250, 25), strFormat);

            var myEncoder = System.Drawing.Imaging.Encoder.Quality;
            var myEncoderParameters = new EncoderParameters(1);

            var myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            //myBitmap.Save(@"c:\Barcode.jpg", jgpEncoder, myEncoderParameters);
            
            picturebox.Image = myBitmap;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            var codecs = ImageCodecInfo.GetImageDecoders();

            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
