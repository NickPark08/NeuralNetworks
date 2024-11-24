using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class NeuralNetwork
    {
        Layer[] layers;
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
            return null;
        }
        public double GetError(double[] inputs, double[] desiredOutputs) 
        {
            return 0;
        }
    }
}
