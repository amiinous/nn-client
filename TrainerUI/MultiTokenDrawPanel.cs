using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MnistTestUi
{
	public class MultiTokenDrawPanel : DrawPanel
	{
		internal List<byte[]> GetNumberBytes(Size imageSize)
		{
			if (Image == null)
				return null;

			//Splits it into tokens
			var tokens = new List<List<(int X, int Y, Color Color)>>();
			var visited = new bool[Image.Width, Image.Height];
			var token = new List<(int X, int Y, Color Color)>();
			for (int x = 0; x < Image.Width; x++)
			{
				for (int y = 0; y < Image.Height; y++)
				{
					var color = Image.GetPixel(x, y);
					if (!visited[x, y] && color != EmptyPixel)
					{
						tokens.Add(FindTokenFrom(x, y, color, visited));
						y = 0;
						x = 0;
					}
					visited[x, y] = true;
				}
			}
			return PreprocessTokens(tokens);
			
			//just for test
			//var colors = new[] { Color.Red, Color.Blue, Color.Green, Color.Pink, Color.Brown, Color.Yellow };
			//for (int i = 0; i < tokens.Count; i++)
			//{
			//	foreach (var pixel in tokens[i])
			//	{
			//		Image.SetPixel(pixel.X, pixel.Y, colors[i]);
			//	}
			//}
			//Invalidate();
		}

		private List<byte[]> PreprocessTokens(List<List<(int X, int Y, Color Color)>> tokens)
		{
			var number = new List<byte[]>();
			foreach (var toke in tokens)
			{
				var xs = toke.Select(t => t.X);
				var ys = toke.Select(t => t.Y);
				var minX = xs.Min();
				var minY = ys.Min();
				var w = xs.Max() - xs.Min() + 1;
				var h = ys.Max() - ys.Min() + 1;
				var bmp = new DirectBitmap(w, h);
				foreach (var p in toke)
				{
					bmp.SetPixel(p.X - minX, p.Y - minY, p.Color);
				}
				var bytes = PreprocessImage(bmp, new Size(28, 28), false);
				number.Add(bytes);
			}
			return number;
		}

		private Color EmptyPixel = Color.FromArgb(0, 0, 0, 0);

		private List<(int, int, Color)> FindTokenFrom(int startX, int startY, Color color, bool[,] visited)
		{
			var token = new List<(int, int, Color)>();
			token.Add((startX, startY, Image.GetPixel(startX, startY)));
			var stack = new Stack<(int X, int Y, Color Color)>();
			stack.Push((startX, startY, color));
			while (stack.Count > 0)
			{
				var pixel = stack.Pop();
				for (int dx = -1; dx < 2; dx++)
				{
					for (int dy = -1; dy < 2; dy++)
					{
						var x = pixel.X + dx;
						var y = pixel.Y + dy;
						if (y < 0 || x < 0 || y > Image.Height - 1 || x > Image.Width - 1)
							continue;

						var c = Image.GetPixel(x, y);
						if (!visited[x, y] && c != EmptyPixel)
						{
							stack.Push((x, y, c));
							token.Add((x, y, c));
						}
						visited[x, y] = true;
					}
				}
			}

			return token;
		}
	}
}
