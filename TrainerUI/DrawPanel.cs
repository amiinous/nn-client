using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MnistTestUi
{
	public class DrawPanel : Panel
	{
		public DrawPanel()
		{
			this.MouseDown += DrawPanel_MouseDown;
			this.MouseUp += DrawPanel_MouseUp;
			this.MouseMove += DrawPanel_MouseMove;
			this.Paint += DrawPanel_Paint;
			this.DoubleBuffered = true;
		}
		
		internal byte[] PreprocessImage(DirectBitmap bitmap, Size resizeTo, bool showPreProccessed)
		{
			if (bitmap == null)
				return null;
			var img = PadAndCenterImage(bitmap);
			if (img == null || img.Width < 0 || img.Height < 0)
				return null;

			if (showPreProccessed)
			{
				Image = img;
				Invalidate();
			}

			return img.ToByteArray();
		}

		private bool MouseISDown { get; set; }

		private Point LastDrawnPoint { get; set; }

		private void DrawPanel_Paint(object sender, PaintEventArgs e)
		{
			if (Image == null)
				return;
			var g = e.Graphics;

			var srcRect = new Rectangle(0, 0, Image.Width, Image.Height);
			var dstRect = new Rectangle(0, 0, Width, Height);
			if (Image != null)
				g.DrawImage(Image.Bitmap, dstRect, srcRect, GraphicsUnit.Pixel);
		}

		private Graphics _graphics;

		private Graphics Graphics
		{
			get
			{
				if (_graphics == null)
				{
					Clear();
				}
				return _graphics;
			}
		}

		protected DirectBitmap PadAndCenterImage(DirectBitmap bitmap)
		{
			var drawnRect = bitmap.DrawnSquare();
			if (drawnRect == Rectangle.Empty)
				return null;
			//Graphics.DrawRectangle(Pens.Red, drawnRect);
			var bmp2020 = bitmap.CropToSize(drawnRect, 20, 20);

			//Make image larger and center on center of mass
			var off = bmp2020.GetMassCenterOffset();
			var bmp2828 = new DirectBitmap(28, 28);
			var gfx2828 = Graphics.FromImage(bmp2828.Bitmap);
			gfx2828.DrawImage(bmp2020.Bitmap, 4 - off.X, 4 - off.Y);

			bmp2020.Dispose();
			return bmp2828;
		}

		internal void Clear()
		{
			Image?.Dispose();
			Image = new DirectBitmap(Width, Height);
			_graphics = Graphics.FromImage(Image.Bitmap);
			//_graphics.CompositingMode = CompositingMode.SourceCopy;
			//_graphics.CompositingQuality = CompositingQuality.HighQuality;
			//_graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			_graphics.SmoothingMode = SmoothingMode.AntiAlias;
			//_graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			Invalidate();
		}

		internal DirectBitmap Image { get; set; }

		private void DrawPanel_MouseUp(object sender, MouseEventArgs e)
		{
			MouseISDown = false;
			LastDrawnPoint = Point.Empty;
		}

		private void DrawPanel_MouseMove(object sender, MouseEventArgs e)
		{
			if (MouseISDown)
			{
				if (LastDrawnPoint != Point.Empty)
				{
					var dX = e.X - (double)LastDrawnPoint.X;
					var dY = e.Y - (double)LastDrawnPoint.Y;
					var dMax = Math.Max(Math.Abs(dX), Math.Abs(dY));
					var stepX = dX / dMax;
					var stepY = dY / dMax;
					for (int i = 0; i < Math.Abs(dMax); i++)
					{
						var x = (int)( LastDrawnPoint.X + stepX * i);
						var y = (int)(LastDrawnPoint.Y + stepY * i);
						Graphics.FillEllipse(Brushes.Black, x, y, 10, 10);
					}
				}
				LastDrawnPoint = new Point(e.X, e.Y);
				Invalidate();
			}
		}

		private void DrawPanel_MouseDown(object sender, MouseEventArgs e)
		{
			MouseISDown = true;
		}
		
	}
}
