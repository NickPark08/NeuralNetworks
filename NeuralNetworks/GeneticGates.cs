using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class GeneticGates
    {
        double min;
        double max;
        public NeuralNetwork[] networks;
        public GeneticGates(double min, double max, int netCount, Random random)
        {
            this.min = min;
            this.max = max;
            networks = new NeuralNetwork[netCount];
            for(int i = 0; i < networks.Length; i++)
            {
                networks[i] = new NeuralNetwork(ActivationFunctions.BinaryStep, ErrorFunctions.MSE, random, [2, 3, 1]);
                ;
            }
            ;
        }


        public double Fitness(NeuralNetwork network, double[][] inputs, double[] desiredOutputs)
        {
            double sum = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                sum += -network.GetError(inputs[i], desiredOutputs[i]);
            }
            return sum / inputs.Length;
        }

        public void Mutate(NeuralNetwork net, Random random, double mutationRate)
        {
            foreach (Layer layer in net.layers)
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    if(neuron.dendrites != null)
                    {
                        for (int i = 0; i < neuron.dendrites.Length; i++)
                        {
                            if (random.NextDouble() < mutationRate)
                            {
                                if (random.Next(2) == 0)
                                {
                                    neuron.dendrites[i].Weight += random.NextDouble();
                                }
                                else
                                {
                                    neuron.dendrites[i].Weight *= -1;
                                }
                            }
                        }
                    }

                    if (random.NextDouble() < mutationRate)
                    {
                        if (random.Next(2) == 0)
                        {
                            neuron.bias += random.NextDouble();//(random.NextDouble() * (1.5 - .5)) + .5;
                        }
                        else
                        {
                            neuron.bias *= -1;
                        }
                    }
                }
            }
        }
        public void Crossover(NeuralNetwork winner, NeuralNetwork loser, Random random)
        {
            for (int i = 0; i < winner.layers.Length; i++)
            {
                Layer winLayer = winner.layers[i];
                Layer childLayer = loser.layers[i];

                int cutPoint = random.Next(winLayer.Neurons.Length);
                bool flip = random.Next(2) == 0;

                if (flip)
                {
                    for (int j = 0; j < cutPoint; j++)
                    {
                        Neuron winNeuron = winLayer.Neurons[j];
                        Neuron childNeuron = childLayer.Neurons[j];

                        if (winNeuron.dendrites != null)
                        {
                            //winNeuron.dendrites.CopyTo(childNeuron.dendrites, 0);

                            for (int k = 0; k < winNeuron.dendrites.Length; k++)
                            {
                                childNeuron.dendrites[k] = winNeuron.dendrites[k];
                            }

                            //literally make new array and set each value from specific index
                        }
                        childNeuron.bias = winNeuron.bias;
                    }
                }
                else
                {
                    for (int j = cutPoint; j < winLayer.Neurons.Length; j++)
                    {
                        Neuron winNeuron = winLayer.Neurons[j];
                        Neuron childNeuron = childLayer.Neurons[j];

                        if (winNeuron.dendrites != null)
                        {
                            for (int k = 0; k < winNeuron.dendrites.Length; k++)
                            {
                                childNeuron.dendrites[k] = winNeuron.dendrites[k];
                            }
                        }
                        childNeuron.bias = winNeuron.bias;
                    }
                }
            }
        }

        public void Train((NeuralNetwork net, double fitness)[] population, Random random, double mutationRate)
        {
            Array.Sort(population, (a, b) => b.fitness.CompareTo(a.fitness));

            int start = (int)(population.Length * 0.1);
            int end = (int)(population.Length * 0.9);

            for (int i = start; i < end; i++)
            {
                Crossover(population[random.Next(start)].net, population[i].net, random);
                ;
                Mutate(population[i].net, random, mutationRate);
            }

            for (int i = end; i < population.Length; i++)
            {
                population[i].net.Randomize(random, min, max);
            }
        }

    }
}
