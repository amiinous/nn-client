using DeepLearning;
using DeepLearning.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MnistTestUi
{
	public partial class MnistImagesForm : Form
	{
		public MnistImagesForm()
		{
			InitialzeCheckBoxes();

			InitializeComponent();
			TrainData = Mnist.Data;

			ShowImages();
			panel1.Paint += Panel1_Paint;
			comboBox1.SelectedIndex = 0;
		}

		private void InitialzeCheckBoxes()
		{
			SuspendLayout();
			for (int i = 0; i < 10; i++)
			{
				var check = new CheckBox
				{
					AutoSize = true,
					Location = new Point(300 + 100 * i, 13),
					TabIndex = i,
					Name = $"checkBox{i}",
					Text = i.ToString(),
					Checked = true,
					Visible = true,
					UseVisualStyleBackColor = true,
					Tag = i
				};
				this.Controls.Add(check);
				check.CheckedChanged += Check_CheckedChanged;
			}
			ResumeLayout(true);
		}

		private void Check_CheckedChanged(object sender, EventArgs e)
		{
			ShowImages();
			panel1.Invalidate();
		}

		private void ShowImages()
		{
			var checks = Controls.OfType<CheckBox>().Where(x => x.Checked).Select(c => (int)c.Tag).ToArray();
			var imgCountX = 47;
			var imgCountY = 27;
			var start = (int)(numericUpDown1.Value - 1) * (imgCountX * imgCountY);
			var samples = GetSamples();
			var filteredSamples = samples.Where(t => checks.Contains(t.Label));
			var imgData = filteredSamples.Skip(start).Select(t => t).ToArray();
			var image = new DirectBitmap(imgCountX * 28, imgCountY * 28);
			var even = true;
			for (int j = 0; j < imgCountY; j++)
			{
				for (int i = 0; i < imgCountX; i++)
				{
					if (i + j * 28 > imgData.Length - 1)
						continue;

					for (int y = 0; y < 28; y++)
					{
						for (int x = 0; x < 28; x++)
						{
							var b = (int)(256 * imgData[i + j * 28].Data[y * 28 + x]);
							if (b == 0 && even)
							{
								image.SetPixel(x + i * 28, y + j * 28, Color.White);
							}
							else
								image.SetPixel(x + i * 28, y + j * 28, Color.FromArgb(b, Color.Black));
						}
					}
					even = !even;
				}
			}
			numericUpDown1.Maximum = (filteredSamples.Count() / (imgCountX * imgCountY)) + 1;
			labelPageCount.Text = "of " + numericUpDown1.Maximum;
			Bitmap?.Dispose();
			Bitmap = image;
		}

		private List<Sample> GetSamples()
		{
			if (comboBox1.SelectedIndex == 2)
			{
				GetUnknowns();
				return Unknowns;
			}
			else
			{
				var samps = comboBox1.SelectedIndex == 0 ? TrainData.TrainSamples : TrainData.TestSamples;
				return samps;
			}
		}

		private async void GetUnknowns()
		{
			if (Unknowns == null)
			{
				Unknowns = new List<Sample>();
				MessageBox.Show("Click Ok to run tests! It will take a minute.");
				progressBar1.Visible = true;
				await Task.Run(() => {
					var count = TrainData.TestSamples.Count;
					for (int i = 0; i < count; i++)
					{
						var sample = TrainData.TestSamples[i];
						var result = NeuralNetwork.Query(TrainData.TestSamples[i]).ToList();
						var index = result.IndexOf(result.Max());
						if (index != sample.Label)
							Unknowns.Add(sample);
						if (i % 50 == 0)
							SetProgress(i / (double)count);
					}
				});
				ShowImages();
				progressBar1.Visible = false;

				panel1.Invalidate();
			}
		}

		delegate void SetProgressCallback(double progress);

		private void SetProgress(double progress)
		{
			if (progressBar1.InvokeRequired)
			{
				SetProgressCallback d = new SetProgressCallback(SetProgress);
				this.Invoke(d, new object[] { progress });
			}
			else
			{
				progressBar1.Value = (int)(progressBar1.Maximum * progress);
			}
		}

		private List<Sample> Unknowns { get; set; }

		private DirectBitmap Bitmap { get; set; }
		private TrainData TrainData { get; }
		internal NeuralNetwork NeuralNetwork { get; set; }

		private void Panel1_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawImage(Bitmap.Bitmap, 0, 0);

		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			ShowImages();
			panel1.Invalidate();
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			ShowImages();
			panel1.Invalidate();
		}
	}
}
