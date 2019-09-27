using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace DeepLearning
{
	[Serializable]
	public class NeuralNetwork
	{
		public NeuralNetwork(int rndSeed, params int[] sizes)
		{
			var rnd = new Random(rndSeed);
			var layersCount = sizes.Length;
			if (layersCount < 2)
				throw new ApplicationException("At least two layers are expected. Input and output.");
			if (sizes.Length != 3)
				throw new ApplicationException("This version only support three layers (one hidden layer)");

			//Creating neurons for each layer.
			Layers = new List<Neuron>[layersCount];
			for (int l = 0; l < sizes.Length; l++)
			{
				Layers[l] = new List<Neuron>();
				//Add an extra bias neron for all but output layer
				var layerSize = l == sizes.Length - 1 ? sizes[l] : sizes[l] + 1;
				for (int n = 0; n < layerSize; n++)
				{
					var neuron = new Neuron();
					Layers[l].Add(neuron);
				}
			}

			//Creating weights between neurons.
			for (int l = 0; l < Layers.Count() - 1; l++) //-1 skips last output layer which has no connected weights
			{
				for (int n = 0; n < Layers[l].Count; n++) //looping neurons of a layer
				{
					var fromN = Layers[l][n];
					//Make no connections to last bias neuron for all layers but last.
					var nextLayerCount = Layers[l + 1].Count();
					var toLayerLength = l + 1 == Layers.Count() - 1 ? nextLayerCount : nextLayerCount - 1; 
					for (int t = 0; t < toLayerLength; t++)
					{
						var toN = Layers[l + 1][t];
						fromN.Weights.Add(new Weight(rnd.NextDouble(), toN));
					}
				}
			}
		}

		public Guid Id { get; set; } = Guid.NewGuid();
		public List<Neuron>[] Layers { get; set; }
		List<Neuron> OutputLayer { get { return Layers[Layers.Length - 1]; } }
		public double TotalError { get; private set; }
		public double LearnRate { get; set; } = 0.5;

		public double[] Query(Sample sample)
		{
			Compute(sample, false);
			return OutputLayer.Select(op => op.Value).ToArray();			
		}

		public void Train(Sample sample)
		{
			Compute(sample, true);
		}

		private void Compute(Sample sample, bool train)
		{
#if DEBUG
			if (sample.Data.Length != Layers[0].Count - 1)
				throw new ApplicationException($"Input must have same length ({Layers[0].Count - 1}) as input layer.");
			var outputsLength = OutputLayer.Count;
			if ( sample.Targets != null && sample.Targets.Length != outputsLength)
				throw new ApplicationException($"Targets must have same length ({outputsLength}) as output layer.");
#endif
			for (int i = 0; i < sample.Data.Length; i++)
				Layers[0][i].Value = sample.Data[i];

			//Forward propagation
			for (int l = 0; l < Layers.Length - 1; l++)
			{
				for (int n = 0; n < Layers[l].Count; n++)
				{
					var neuron = Layers[l][n];
					foreach (var weight in neuron.Weights)
						weight.ConnectedNeuron.Value += weight.Value * neuron.Value;
				}

				var neuronCount = Layers[l + 1].Count;
				if (l + 1 < Layers.Count() - 1)
					neuronCount--; //skipping bias
				for (int n = 0; n < neuronCount; n++) //next layer
				{
					var neuron = Layers[l + 1][n];
					neuron.Value = LeakyReLU( neuron.Value / Layers[l].Count);
					//neuron.Value = Sigmoid(neuron.Value / Layers[l].Count);
				}
			}

			if (train)
				ComputeNextWeights(sample.Targets);
		}
		
		[NonSerialized]
		private ParallelOptions _parallelOptions = null;
		private ParallelOptions GetParallelOptions()
		{
			//I use a method beacause properties dont work so well with binary serialization.
			if (_parallelOptions == null)
				_parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 8 };
			return _parallelOptions;
		}

		private void ComputeNextWeights(double[] targets)
		{
			var output = OutputLayer;
			for (int t = 0; t < output.Count; t++)
				output[t].Target = targets[t];

			//backwards propagation
			foreach (var neuron in output)
			{
				neuron.Error = Math.Pow(neuron.Target - neuron.Value, 2) / 2;
				neuron.Delta = (neuron.Value - neuron.Target) * (neuron.Value > 0 ? 1 : 1 / 20d);
				//neuron.Delta = (neuron.Value - neuron.Target) * (neuron.Value * (1 - neuron.Value));
			}
			this.TotalError = output.Sum(n => n.Error);

			
			//Only one hidden layer support
			foreach (var neuron in Layers[1])
			{
				foreach (var weight in neuron.Weights)
					weight.Delta = neuron.Value * weight.ConnectedNeuron.Delta;
			}
			
			//Input Layer
			Parallel.ForEach(Layers[0], GetParallelOptions(), (neuron) => {

				foreach (var weight in neuron.Weights)
				{
					foreach (var connectedWeight in weight.ConnectedNeuron.Weights)
						weight.Delta += connectedWeight.Value * connectedWeight.ConnectedNeuron.Delta;
					var cv = weight.ConnectedNeuron.Value;
					//weight.Delta *= (cv * (1 - cv));
					weight.Delta *= cv > 0 ? 1 : 1 / 20d;
					weight.Delta *= neuron.Value;
				}

			});

			//All deltas are done. Now calculate new weights.
			for (int l = 0; l < Layers.Length - 1; l++)
			{
				var layer = Layers[l];
				foreach (var neuron in layer)
					foreach (var weight in neuron.Weights)
						weight.Value -= (weight.Delta * this.LearnRate);
			}
		}
		
		public void Save(string fileName)
		{
			var serializer = new BinaryFormatter();
			var stream = File.OpenWrite(fileName);
			serializer.Serialize(stream, this);
			stream.Close();
		}

		public static NeuralNetwork Load(string file)
		{
			var serializer = new BinaryFormatter();
			var stream = File.OpenRead(file);
			var obj = serializer.Deserialize(stream);
			stream.Close();
			return (NeuralNetwork)obj;
		}

		/// <summary>
		/// Activation function.
		/// </summary>
		private double LeakyReLU(double x)
		{
			if (x >= 0)
				return x;
			else
				return x / 20;
		}

		private double Sigmoid(double x)
		{
			var s = 1 / (1 + Math.Exp(-x));
			return s;
		}
	}

	public class TrainData
	{
		public List<Sample> TrainSamples { get; set; }
		public List<Sample> TestSamples { get; set; }
		public double[][] Targets { get; set; }

	}

	public struct Sample
	{
		public double[] Targets { get; set; }

		public double[] Data { get; set; }
		public int Label { get; set; }
	}
}
