using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MnistTestUi
{
	public class DirectBitmap : IDisposable
	{
		public Bitmap Bitmap { get; private set; }
		public Int32[] Bits { get; private set; }
		public bool Disposed { get; private set; }
		public int Height { get; private set; }
		public int Width { get; private set; }

		protected GCHandle BitsHandle { get; private set; }

		public DirectBitmap(int width, int height)
		{
			Width = width;
			Height = height;
			Bits = new Int32[width * height];
			BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
			Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
		}

		public void SetPixel(int x, int y, Color colour)
		{
			int index = x + (y * Width);
			int col = colour.ToArgb();

			Bits[index] = col;
		}

		public Color GetPixel(int x, int y)
		{
			int index = x + (y * Width);
			int col = Bits[index];
			Color result = Color.FromArgb(col);

			return result;
		}

		public void Dispose()
		{
			if (Disposed) return;
			Disposed = true;
			Bitmap.Dispose();
			BitsHandle.Free();
		}

		/// <summary>
		/// Finds smallest square around the drawing.
		/// </summary>
		/// <returns></returns>
		public Rectangle DrawnSquare()
		{
			var fromX = int.MaxValue;
			var toX = int.MinValue;
			var fromY = int.MaxValue;
			var toY = int.MinValue;
			var empty = true;
			for (int y = 0; y < Bitmap.Height; y++)
			{
				for (int x = 0; x < Bitmap.Width; x++)
				{
					var pixel = Bitmap.GetPixel(x, y);
					if (pixel.A > 0)
					{
						empty = false;
						if (x < fromX)
							fromX = x;
						if (x > toX)
							toX = x;
						if (y < fromY)
							fromY = y;
						if (y > toY)
							toY = y;
					}
				}
			}
			if (empty)
				return Rectangle.Empty;
			var dx = toX - fromX;
			var dy = toY - fromY;
			var side = Math.Max(dx, dy);
			if (dy > dx)
				fromX -= (side - dx) / 2;
			else
				fromY -= (side - dy)/ 2;

			return new Rectangle(fromX, fromY, side, side);
		}

		/// <summary>
		/// Crops a portion of the bitmap and return a new bitmap with a new size.
		/// </summary>
		/// <param name="drawnRect"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public DirectBitmap CropToSize(Rectangle drawnRect, int width, int height)
		{
			var bmp = new DirectBitmap(width, height);
			bmp.Bitmap.SetResolution(Bitmap.HorizontalResolution, Bitmap.VerticalResolution);

			var gfx = Graphics.FromImage(bmp.Bitmap);
			gfx.CompositingQuality = CompositingQuality.HighQuality;
			gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
			gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
			gfx.SmoothingMode = SmoothingMode.AntiAlias;
			var rect = new Rectangle(0, 0, width, height);
			//using (var wrapMode = new ImageAttributes())
			//{
			//	wrapMode.SetWrapMode(WrapMode.Tile);
			//	gfx.DrawImage(Bitmap, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
			//}
			gfx.DrawImage(Bitmap, rect, drawnRect, GraphicsUnit.Pixel);
			return bmp;
		}
		
		/// <summary>
		/// Returns the offset of mass center of the drawing.
		/// </summary>
		/// <returns></returns>
		public Point GetMassCenterOffset()
		{
			var path = new List<Vector2>();
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					var c = GetPixel(x, y);
					if (c.A > 0)
						path.Add(new Vector2(x, y));
				}
			}
			var centroid = path.Aggregate(Vector2.Zero, (current, point) => current + point) / path.Count();
			return new Point((int)centroid.X - Width / 2, (int)centroid.Y - Height / 2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		public byte[] ToByteArray()
		{
			var bytes = new List<byte>();
			for (int y = 0; y < Bitmap.Height; y++)
			{
				for (int x = 0; x < Bitmap.Width; x++)
				{
					var color = Bitmap.GetPixel(x, y);
					var i = color.A;
					bytes.Add(i);
				}
			}
			return bytes.ToArray();
		}
	}
}
