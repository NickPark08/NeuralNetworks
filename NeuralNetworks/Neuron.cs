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
        public double Delta { get; set; }
        double biasUpdate;
        public ActivationFunction Activation { get; set; }

        public Neuron(ActivationFunction activation, Neuron[] previousNeurons)
        {
            if (previousNeurons != null)
            {
                dendrites = new Dendrite[previousNeurons.Length];
                for (int i = 0; i < dendrites.Length; i++)
                {
                    dendrites[i] = new Dendrite(previousNeurons[i], this, 0);
                }
            }
            Activation = activation;
        }
        public void Randomize(Random random, double min, double max)
        {
            bias = (random.NextDouble() * (max - min)) + min;

            if (dendrites != null)
            {
                foreach (var dendrite in dendrites)
                {
                    dendrite.Weight = (random.NextDouble() * (max - min)) + min;
                }
            }
        }
        public double Compute()
        {
            Input = bias;
            for (int i = 0; i < dendrites.Length; i++)
            {
                Input += dendrites[i].Compute();
            }

            Output = Activation.Function(Input);

            return Output;
        }
        public void ApplyUpdate()
        {
            bias += biasUpdate;
            biasUpdate = 0; 
            if (dendrites != null)
            {
                foreach (var dendrite in dendrites)
                {
                    dendrite.ApplyUpdate();
                }
            }
        }

        public void Backprop(double learningRate)
        {
            for(int i = 0; i < dendrites.Length; i++)
            {
                dendrites[i].Previous.Delta += Delta * Activation.Derivative(Input) * dendrites[i].Weight;
                dendrites[i].WeightUpdate += learningRate * -(Delta * Activation.Derivative(Input) * dendrites[i].Previous.Output);
            }
            biasUpdate += learningRate * -(Delta * Activation.Derivative(Input));
            Delta = 0;
        }
    }
}
