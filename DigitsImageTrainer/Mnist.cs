using DeepLearning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DeepLearning.Data
{
	public class Mnist
	{

		private static double[][] GetTargets()
		{
			var targets = new double[10][];
			for (int i = 0; i < 10; i++)
			{
				targets[i] = new double[10];
				//for (int j = 0; j < 10; j++)
				//	Targets[i][j] = 0.01;
				targets[i][i] = 1;
			}
			return targets;
		}
		private static List<Sample> LoadMnistImages(string imgFileName, string idxFileName, int imgCount)
		{
			var imageReader = File.OpenRead(imgFileName);
			var byte4 = new byte[4];
			imageReader.Read(byte4, 0, 4); //magic number
			imageReader.Read(byte4, 0, 4); //magic number
			Array.Reverse(byte4);
			//var imgCount = BitConverter.ToInt32(byte4, 0);

			imageReader.Read(byte4, 0, 4); //width (28)
			imageReader.Read(byte4, 0, 4); //height (28)
			var samples = new Sample[imgCount];

			var labelReader = File.OpenRead(idxFileName);
			labelReader.Read(byte4, 0, 4);//magic number
			labelReader.Read(byte4, 0, 4);//count
			var targets = GetTargets();

			for (int i = 0; i < imgCount; i++)
			{
				samples[i].Data = new double[784];
				var buffer = new byte[784];
				imageReader.Read(buffer, 0, 784);
				for (int b = 0; b < buffer.Length; b++)
					samples[i].Data[b] = buffer[b] / 256d;

				//todo: This might get better training
				//images[i].Data[b] = buffer[b] == 0d ? 0.01 : buffer[b] / 256d; 
				samples[i].Label = labelReader.ReadByte();
				samples[i].Targets = targets[samples[i].Label];
			}
			return samples.ToList();
		}

		private static TrainData _data;
		public static TrainData Data
		{
			get
			{
				if (_data == null)
				{
					var trainData = new TrainData
					{
						TrainSamples = LoadMnistImages("train-images-idx3-ubyte", "train-labels-idx1-ubyte", 60000),
						TestSamples = LoadMnistImages("t10k-images-idx3-ubyte", "t10k-labels-idx1-ubyte", 10000)
					};
					_data = trainData;
				}
				return _data;
			}
		}
	}
}