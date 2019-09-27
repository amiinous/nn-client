using DeepLearning;
using DeepLearning.Data;
using System;

namespace DeepLearingConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			NeuralNetwork neuralNetwork;
			string file = args != null && args.Length > 0 ? args[0] : null;
			if (string.IsNullOrEmpty(file))
			{
				neuralNetwork = new NeuralNetwork(rndSeed: 0, sizes: new[] { 784, 200, 10 });
			}
			else
			{
				neuralNetwork = NeuralNetwork.Load(file);
			}
			neuralNetwork.LearnRate = 0.3;

			var trainer = new Trainer(neuralNetwork, Mnist.Data);
			trainer.Train();
		}
	}
}
