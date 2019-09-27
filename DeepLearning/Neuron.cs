using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeepLearning
{
	[Serializable]
	public class Neuron
	{
		public double Value { get; set; } = 1;
		public double Target { get; set; }
		public double Error { get; set; }
		public double Delta { get; set; }
		
		/// <summary>
		/// Weights from this neuron to other neurons
		/// </summary>
		public List<Weight> Weights = new List<Weight>();
		
	}


	[Serializable]
	public class Weight
	{
		public Weight(double initValue, Neuron connectedNeuron)
		{
			Value = initValue;
			ConnectedNeuron = connectedNeuron;
		}
		public double Value { get; set; }
		public Neuron ConnectedNeuron { get; set; }
		public double Delta { get; set; }		
	}

}
