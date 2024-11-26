using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class Neuron
    {
        public double bias;
        public Dendrite[] dendrites;
        public double Output { get; set; }
        public double Input { get; private set; }
        public ActivationFunction Activation { get; set; }

        public Neuron(ActivationFunction activation, Neuron[] previousNeurons)
        {
            dendrites = new Dendrite[previousNeurons.Length];
            Activation = activation;
            for(int i = 0; i < previousNeurons.Length; i++)
            {
                for (int j = 0; j < previousNeurons.Length; j++)
                {
                    if (j != i)
                    {
                        dendrites[i] = new Dendrite(previousNeurons[i], previousNeurons[j], 0);
                    }
                }
            }
        }
        public void Randomize(Random random, double min, double max)
        {
            bias = (random.NextDouble() * (max - min)) + min;

            foreach(var dendrite in dendrites)
            {
                dendrite.Weight = (random.NextDouble() * (max - min)) + min;
            }
        }
        public double Compute()
        {
            Input = 0;
            for(int i = 0; i < dendrites.Length; i++)
            {
                Input += dendrites[i].Weight * dendrites[i].Previous.Output;
            }
            Input += bias;

            Output = Activation.Function(Input);

            return Output;
        }
    }
}
