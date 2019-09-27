using DeepLearning;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
	public class NeuralNetworkTests
	{
		public NeuralNetworkTests(ITestOutputHelper output)
		{
			Output = output;
		}

		private readonly ITestOutputHelper Output;


		[Fact]
		public void TestCreate()
		{
			var network = new NeuralNetwork(1234, 2, 3, 2);
			Assert.Equal(3, network.Layers[0].Count);
			Assert.Equal(4, network.Layers[1].Count);
			Assert.Equal(2, network.Layers[2].Count);
			Assert.Equal(3, network.Layers[0][0].Weights.Count);
			Assert.Equal(2, network.Layers[1][0].Weights.Count);
		}

		[Fact]
		public void TestCompute1()
		{
			var network = new NeuralNetwork(1234, 2, 2, 2);
			var sample = new Sample
			{
				Data = new[] { 0.8d, 0.4 },
				Targets = new[] { 0.1d, 0.88 }
			};
			network.Train(sample);
			var s = new StringBuilder(); ;
			foreach (var layer in network.Layers)
				foreach (var neuron in layer)
					foreach (var weight in neuron.Weights)
					{
						s.Append(weight.Value);
						s.AppendLine();
					}
			Output.WriteLine(s.ToString());
			
		}

		[Fact]
		public void TestSave()
		{
			var network = new NeuralNetwork(1234, 5, 6, 7);
			network.Save("nn.bin");

			var loaded = NeuralNetwork.Load("nn.bin");
			//Assert.Equal(3, loaded.Layers.Length);
			//Assert.Equal(network.Weights[1][1][1], network.Weights[1][1][1]);
		}
		
		[Fact]
		public void TestLearn1()
		{
			var network = new NeuralNetwork(1234, 2, 2, 2);
			network.Layers[0][0].Weights[0].Value = 0.15;
			network.Layers[0][0].Weights[1].Value = 0.25;

			network.Layers[0][1].Weights[0].Value = 0.20;
			network.Layers[0][1].Weights[1].Value = 0.30;


			network.Layers[1][0].Weights[0].Value = 0.40;
			network.Layers[1][0].Weights[1].Value = 0.50;
			network.Layers[1][1].Weights[0].Value = 0.45;
			network.Layers[1][1].Weights[1].Value = 0.55;

			var sample = new Sample
			{
				Data = new[] { 0.3d, 0.4 },
				Targets = new[] { 0.01d, 0.99 }
			};
			//osv
			for (int i = 0; i < 100; i++)
			{
				network.Train(sample);
				Output.WriteLine(network.TotalError.ToString() + ";" + network.Layers[2][0].Value + ";" + network.Layers[2][1].Value);
			}
		}

		[Fact]
		public void TestLearn2()
		{
			var network = new NeuralNetwork(1234, 3, 5, 3);
			network.LearnRate = 0.1;
			//osv
			var sample = new Sample
			{
				Data = new[] { 0.3d, 0.4, 0.5 },
				Targets = new[] { 0.01d, 0.77, 0.99 }
			};
			for (int i = 0; i < 5000; i++)
			{
				network.Train(sample);
				Output.WriteLine(network.TotalError.ToString() + ";" 
					+ network.Layers[2][0].Value + ";" 
					+ network.Layers[2][1].Value + ";" 
					+ network.Layers[2][2].Value);
			}
		}
	}
}
