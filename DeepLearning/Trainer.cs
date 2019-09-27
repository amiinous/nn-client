using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DeepLearning
{
	public class Trainer
	{
		public Trainer(NeuralNetwork neuralNetwork, TrainData trainData)
		{
			NeuralNetwork = neuralNetwork;
			TrainData = trainData;
		}
		private TrainData TrainData { get; } 
		private NeuralNetwork NeuralNetwork { get; }

		public void Train(int epochs = 100)
		{			
			var rnd = new Random(0);
			var name = $"LeakyRelu LR{NeuralNetwork.LearnRate} HL{NeuralNetwork.Layers[1].Count}";
			var csvFile = $"{name}.csv";
			var bestResult = 0d;
			for (int epoch = 1; epoch < epochs; epoch++)
			{
				Shuffle(TrainData.TrainSamples, rnd);
				TrainEpoch();				
				var result = Test();
				Log($"Epoch {epoch} {result.ToString("P")}");
				File.AppendAllText(csvFile, $"{epoch};{result};{NeuralNetwork.TotalError}\r\n");
				if (result > bestResult)
				{
					NeuralNetwork.Save($"{name}.bin");
					Log($"Saved {name}.bin");
					bestResult = result;
				}
			}
		}

		private void Log(string message)
		{
			Console.WriteLine(message);
		}

		private double Test()
		{
			var match = 0d;
			var sample = TrainData.TestSamples;
			for (int i = 0; i < sample.Count; i++)
			{
				var output = NeuralNetwork.Query(sample[i]);
				var max = output.Max();
				int digit = output.ToList().IndexOf(max);
				var expectedMax = sample[i].Targets.Max();
				var expectedDigit = sample[i].Targets.ToList().IndexOf(expectedMax);
				if (digit == expectedDigit)
					match++;
			}
			return match / sample.Count;
		}

		private static void Shuffle<T>(IList<T> list, Random rnd)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rnd.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		private void TrainEpoch(int? iterations = null)
		{
			var tmpError = 0d;
			var stopWatch = Stopwatch.StartNew();
			iterations = iterations ?? TrainData.TrainSamples.Count;
			for (int i = 0; i < iterations; i+=1)
			{
				NeuralNetwork.Train(TrainData.TrainSamples[i]);
				if (i % 1000 == 0 && i != 0)
				{
					var speed = (int)(1000 / stopWatch.Elapsed.TotalSeconds);
					Log(i + ". " + (tmpError / 1000).ToString("P") + " " + speed + " i/s");
					stopWatch.Restart();
					tmpError = 0;
				}
				tmpError += NeuralNetwork.TotalError;
			}
		}
	}
}