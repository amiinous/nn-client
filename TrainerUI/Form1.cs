using DeepLearning;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MnistTestUi
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			NeuralNetwork = NeuralNetwork.Load("LeakyRelu_LR05_HL201.bin");
		}
		
		public NeuralNetwork NeuralNetwork { get; set; }

		private void openButton_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				NeuralNetwork = NeuralNetwork.Load(openFileDialog1.FileName);
			}
		}

		private void buttonClear_Click(object sender, EventArgs e)
		{
			drawPanel.Clear();
			multiTokenDrawPanel1.Clear();
			textBoxParsedNumber.Text = "";
			textBoxParsedDigit.Text = "";
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (NeuralNetwork == null)
				return;

			var bytes = drawPanel.PreprocessImage(drawPanel.Image, new Size(28,28), true);
			if (bytes != null)
			{
				var input = bytes.Select(b => (double)b / 256).ToArray();
               
                Console.WriteLine(String.Join(",", bytes.Select(p => p.ToString()).ToArray()));
                Console.WriteLine(input.Length);

                

                var sample = new Sample	{ Data = input };
				var r = NeuralNetwork.Query(sample).ToList();
				var max = r.Max();
				var idx = r.IndexOf(max);
				textBoxParsedDigit.Text = idx.ToString() + "\r\n";

                using (StreamWriter writetext = new StreamWriter("gen.t", true))
                {
                    writetext.Write(idx.ToString());
                    //writetext.Write("0"); //In future the software NN will be removed so the idx is meaningless
                    for (int index = 0; index < bytes.Length; index++)
                    {
                        var val = bytes[index];

                        if(val!=0)
                        {
                            writetext.Write(' ' + (index+1).ToString() + ':' + bytes[index].ToString());
                        }
                    }
                    writetext.WriteLine("");
                }

                var s = "";
				for (int i = 0; i < 10; i++)
					s += $"{i}: {r[i].ToString("0.000")}\r\n";
				textBoxParsedDigit.Text += s;
			}

			var numberBytes = multiTokenDrawPanel1.GetNumberBytes(new Size(28, 28));
			if (numberBytes == null)
				return;
			var sb = new StringBuilder();
			foreach (var byts in numberBytes)
			{
				var input = byts.Select(b => (double)b / 256).ToArray();
				var sample = new Sample { Data = input };

				var r = NeuralNetwork.Query(sample).ToList();
				var max = r.Max();
				var idx = r.IndexOf(max);
				sb.Append(idx.ToString());
			}
			textBoxParsedNumber.Text = sb.ToString();
		}
	
		private void showButton_Click(object sender, EventArgs e)
		{
			var form = new MnistImagesForm();
			form.NeuralNetwork = this.NeuralNetwork;
			form.Show(this);
		}
	}
}
