using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class Layer
    {
        public Neuron[] Neurons { get; }
        public double[] Outputs { get; }

        public Layer(ActivationFunction activation, int neuronCount, Layer previousLayer)
        {
            Neurons = new Neuron[neuronCount];
            Outputs = new double[neuronCount];
            Neuron[] previousNeurons = null;
            if (previousLayer != null)
            {
                previousNeurons = previousLayer.Neurons;
            }

            for (int i = 0; i < neuronCount; i++)
            {
                Neurons[i] = new Neuron(activation, previousNeurons);
            }
        }
        public void Randomize(Random random, double min, double max)
        {
            foreach (Neuron neuron in Neurons)
            {
                neuron.Randomize(random, min, max);
            }
        }
        public double[] Compute()
        {
            for (int i = 0; i < Outputs.Length; i++)
            {
                Outputs[i] = Neurons[i].Compute();
            }
            return Outputs;
        }
    }
}
