using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace baUHInia.Playground.Logic.Utils
{
    public static class ImageWriter
    {
        public static RenderTargetBitmap CopyAsBitmap(FrameworkElement frameworkElement)
        {
            int targetWidth = (int) frameworkElement.ActualWidth;
            int targetHeight = (int) frameworkElement.ActualHeight;

            if (targetWidth == 0 || targetHeight == 0) return null;

            var result = new RenderTargetBitmap(targetWidth, targetHeight, 96, 96, PixelFormats.Pbgra32);
            result.Render(frameworkElement);
            return result;
        }

        public static byte[] Encode(BitmapSource bitmapSource, BitmapEncoder bitmapEncoder)
        {
            var bitmapFrame = BitmapFrame.Create(bitmapSource);
            bitmapEncoder.Frames.Add(bitmapFrame);
            
            var memoryStream = new MemoryStream();
            bitmapEncoder.Save(memoryStream);
            
            return memoryStream.ToArray();
        }
    }
}