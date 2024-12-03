using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class NeuralNetwork
    {
        public Layer[] layers;
        ErrorFunction errorFunc;

        public NeuralNetwork(ActivationFunction activation, ErrorFunction errorFunc, params int[] neuronsPerLayer)
        {
            this.errorFunc = errorFunc;
            layers = new Layer[neuronsPerLayer.Length];
            layers[0] = new Layer(activation, neuronsPerLayer[0], null);
            for (int i = 1; i < layers.Length; i++)
            {
                layers[i] = new Layer(activation, neuronsPerLayer[i], layers[i-1]);
            }
        }
        public void Randomize(Random random, double min, double max) 
        {
            foreach (var layer in layers)
            {
                layer.Randomize(random, min, max);
            }
        }
        public double[] Compute(double[] inputs) 
        {
            double[] output = new double[layers[0].Outputs.Length];
            for(int i = 0; i < layers[0].Outputs.Length; i++)
            {
                layers[0].Neurons[i].Output = inputs[i];
            }
            for(int i = 1; i < layers.Length; i++)
            {
                output = layers[i].Compute();
            }
            return output;
        }
        public double GetError(double[] inputs, double[] desiredOutputs) 
        {
            double sum = 0;
            double[] computedInputs = Compute(inputs);
            for (int i = 0; i < inputs.Length; i++)
            {
                sum += errorFunc.Function(inputs[i], desiredOutputs[i]);
            }
            return sum / desiredOutputs.Length;
        }
    }
}
