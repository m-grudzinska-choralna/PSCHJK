using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KS2
{
    static class ObrobkaGrafiki
    {
        public static Bitmap PanelToBitmap(Panel pnl)
        {
            Bitmap bmp = new Bitmap(pnl.Width, pnl.Height);
            pnl.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height)); 
            return bmp;
        }


        public static Bitmap CropBitmap(Bitmap bitmap, int cropX1, int cropY1, int cropX2, int cropY2)
        {

            Rectangle rect = new Rectangle(cropX1, cropY1, cropX2 - cropX1 + 1, cropY2 - cropY1 + 1);
            Bitmap cropped = bitmap.Clone(rect, bitmap.PixelFormat);
            return cropped;
        }

        public static Image ZmienKolor(Image img,Color color)
        {
            Bitmap bmp = (Bitmap)img;
            for (int x = 0; x < img.Width; x++)
                for (int y = 0; y < img.Height; y++)
                {
                    if (bmp.GetPixel(x,y).A>200)
                    bmp.SetPixel(x, y, color);
                }
            return bmp;
        }
    }
}
