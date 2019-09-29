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
using System.Net.Http;
using RestSharp;

namespace MnistTestUi
{
	public partial class Form1 : Form
	{
        private static readonly HttpClient client = new HttpClient();
        public Form1()
		{
			InitializeComponent();
            label4.Text = "";
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
            label4.Text = "";
		}

		private async void button1_Click(object sender, EventArgs e)
		{
			if (NeuralNetwork == null)
				return;

            
            var bytes = drawPanel.PreprocessImage(drawPanel.Image, new Size(28,28), false);
			if (bytes != null)
			{
                label4.Text = ".";
                var input = bytes.Select(b => (double)b / 256).ToArray();

                var sample = new Sample	{ Data = input };
				var r = NeuralNetwork.Query(sample).ToList();
				var max = r.Max();
				var idx = r.IndexOf(max);


                //using (StreamWriter writetext = new StreamWriter("gen.t", true))
                //{
                    
                //    writetext.Write(idx.ToString());
                //    writetext.Write("0"); //In future the software NN will be removed so the idx is meaningless
                //    for (int index = 0; index < bytes.Length; index++)
                //    {
                //        var val = bytes[index];

                //        if(val!=0)
                //        {
                //            writetext.Write(' ' + (index+1).ToString() + ':' + bytes[index].ToString());
                            
                //        }
                //    }
                //    writetext.WriteLine("");
                //}

                var sampled = idx.ToString();
                for (int index = 0; index < bytes.Length; index++)
                {
                    var val = bytes[index];

                    if (val != 0)
                        sampled += ' ' + (index + 1).ToString() + ':' + bytes[index].ToString();
                    
                }

                var client = new RestClient("http://192.168.1.177:8181/");
                var request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                var arrayOfDigits = new String[] { sampled };
                request.AddJsonBody(new { digits = arrayOfDigits });
                var cancellationTokenSource = new System.Threading.CancellationTokenSource();
                var response = await client.ExecuteTaskAsync(request, cancellationTokenSource.Token);
                label4.Text = response.Content;

                var s = "";
				for (int i = 0; i < 10; i++)
					s += $"{i}: {r[i].ToString("0.000")}\r\n";
			}
		}
	
		private void showButton_Click(object sender, EventArgs e)
		{
			var form = new MnistImagesForm();
			form.NeuralNetwork = this.NeuralNetwork;
			form.Show(this);
		}

        private async void button2_Click(object sender, EventArgs e)
        {
            
            var numberBytes = multiTokenDrawPanel1.GetNumberBytes(new Size(28, 28));
         
            if (numberBytes == null || numberBytes.Count == 0)
                return;
            label4.Text = ".";
            var sb = new StringBuilder();
            var arrayOfDigits2 = new List<string>();
            foreach (var byts in numberBytes)
            {
                var input = byts.Select(b => (double)b / 256).ToArray();
                var sample = new Sample { Data = input };

                var r = NeuralNetwork.Query(sample).ToList();
                var max = r.Max();
                var idx = r.IndexOf(max);
                sb.Append(idx.ToString());

                var sampledMulti = idx.ToString();
                for (int index = 0; index < byts.Length; index++)
                {
                    var val = byts[index];

                    if (val != 0)
                        sampledMulti += ' ' + (index + 1).ToString() + ':' + byts[index].ToString();

                }
                arrayOfDigits2.Add(sampledMulti);

            }
            
            var client = new RestClient("http://192.168.1.177:8181/");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddJsonBody(new { digits = arrayOfDigits2.ToArray() });
            var cancellationTokenSource = new System.Threading.CancellationTokenSource();
            var response = await client.ExecuteTaskAsync(request, cancellationTokenSource.Token);

            label4.Text = response.Content;
        }
    }
}
