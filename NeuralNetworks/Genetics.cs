using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class Genetics
    {
        double min;
        double max;
        public int NetCount;
        public Genetics(double min, double max, int netCount, Random random)
        {
            this.min = min;
            this.max = max;
            NetCount = netCount;
        }

        public double SineWaveFitness(NeuralNetwork network, double[][] inputs, double[][] outputs)
        {
            double sum = 0;
            for(int i = 0; i < inputs.Length; i++)
            {
                sum += -Math.Abs(Math.Sin(inputs[i][0] / 10) - network.Compute(outputs[i])[0]);
            }

            return sum;
        }
        
        public double FlappyBirdFitness(double time, int score)
        {
            return -time / (score + 1); // maybe adjust? start training in Game1
        }

        public double GateFitness(NeuralNetwork network, double[][] inputs, double[] desiredOutputs)
        {
            double sum = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                if (network.Compute(inputs[i])[0] == desiredOutputs[i])
                {
                    sum++;
                }
            }
            
            return sum;
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
                                childNeuron.dendrites[k].Weight = winNeuron.dendrites[k].Weight;


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
                                childNeuron.dendrites[k].Weight = winNeuron.dendrites[k].Weight;
                            }
                        }
                        childNeuron.bias = winNeuron.bias;
                    }
                }
            }
        }

        public Population[] Train(Population[] population, Random random, double mutationRate)
        {
            Array.Sort(population, (a, b) => b.Fitness.CompareTo(a.Fitness));

            int start = (int)(population.Length * 0.1);
            int end = (int)(population.Length * 0.9);

            for (int i = start; i < end; i++)
            {
                Crossover(population[random.Next(start)].Network, population[i].Network, random);
                ;
                Mutate(population[i].Network, random, mutationRate);
            }

            for (int i = end; i < population.Length; i++)
            {
                population[i].Network.Randomize(random, min, max);
            }
            return population;
        }

    }
}
