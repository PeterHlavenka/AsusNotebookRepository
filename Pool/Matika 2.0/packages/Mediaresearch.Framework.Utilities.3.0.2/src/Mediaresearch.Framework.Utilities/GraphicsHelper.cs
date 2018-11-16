using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Mediaresearch.Framework.Utilities
{
	public static class GraphicsHelper
	{
		public static Bitmap CreateGrayscaleIndexedBitmap(int width, int height, Stream bitmapData)
		{
			Bitmap result = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
			SetGrayscalePalette(result);

			BitmapData data =
				result.LockBits(new Rectangle(0, 0, result.Width, result.Height), ImageLockMode.ReadWrite,
				                PixelFormat.Format8bppIndexed);

			IntPtr ptr = data.Scan0;
			unsafe
			{
				byte* pBits;
				if (data.Stride > 0)
				{
					pBits = (byte*) ptr.ToPointer();
				}
				else
				{
					// If the Stide is negative, Scan0 points to the last
					// scanline in the buffer. To normalize the loop, obtain
					// a pointer to the front of the buffer that is located
					// (Height-1) scanlines previous.
					pBits = (byte*) ptr.ToPointer() +
					        data.Stride*(height - 1);
				}
				uint stride = (uint) Math.Abs(data.Stride);
				//float fStep = (float) (256.0/width);
				for (uint row = 0; row < height; ++row)
				{
					for (uint col = 0; col < width; ++col)
					{
						byte* p8bppPixel = pBits + row*stride + col;
						*p8bppPixel = (byte) bitmapData.ReadByte();
					}
				}
			}

			result.UnlockBits(data);

			return result;
		}

		public static void SetGrayscalePalette(Bitmap image)
		{
			if (image.PixelFormat != PixelFormat.Format8bppIndexed)
				throw new ArgumentException();

			ColorPalette cp = image.Palette;

			for (int i = 0; i < 256; i++)
			{
				cp.Entries[i] = Color.FromArgb(i, i, i);
			}

			image.Palette = cp;
		}
	}
}