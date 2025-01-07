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

        public NeuralNetwork(ActivationFunction activation, ErrorFunction error, Random random, params int[] neuronsPerLayer)
        {
            errorFunc = error;
            layers = new Layer[neuronsPerLayer.Length];
            Layer previousLayer = default;
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = new Layer(activation, neuronsPerLayer[i], previousLayer);
                previousLayer = layers[i];
            }
            Randomize(random, 0, 1);
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
            for(int i = 0; i < layers[0].Neurons.Length; i++)
            {
                layers[0].Neurons[i].Output = inputs[i];
            }

            for(int i = 1; i < layers.Length; i++)
            {
                output = layers[i].Compute();
            }
            return output;
        }

        public void ApplyUpdate()
        {
            for(int i = 1; i < layers.Length; i++)
            {
                layers[i].ApplyUpdate();
            }
        }
        public void Backprop(double learningRate, double[] desiredOutputs)
        {
            Layer outputLayer = layers[layers.Length - 1];
            for(int i = 0; i < outputLayer.Neurons.Length; i++)
            {
                outputLayer.Neurons[i].Delta += errorFunc.Derivative(outputLayer.Neurons[i].Output, desiredOutputs[i]);
            }

            for(int i = layers.Length - 1; i > 0; i--)
            {
                layers[i].Backprop(learningRate);
            }
        }

        public double GetError(double[] inputs, double[] desiredOutputs)
        {
            double[] computedInputs = Compute(inputs);
            double error = 0;
            for (int i = 0; i < computedInputs.Length; i++)
            {
                error += errorFunc.Function(computedInputs[i], desiredOutputs[i]);
            }
            return error;
        }

        public double Train(double[][] inputs, double[][] desiredOutputs, double learningRate)
        {
            double error = 0;
            for(int i = 0; i < inputs.Length; i++)
            {
                error += GetError(inputs[i], desiredOutputs[i]);
                Backprop(learningRate, desiredOutputs[i]);
            }

            ApplyUpdate();
            return error / inputs.Length;
        }
    }
}
