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

        public double Fitness(NeuralNetwork network, double[] inputs, double[] desiredOutpus)
        {
            return -network.GetError(inputs, desiredOutpus);
        }

        public void Mutate(NeuralNetwork net, Random random, double mutationRate)
        {
            foreach (Layer layer in net.layers)
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    //Mutate the Weights
                    for (int i = 0; i < neuron.dendrites.Length; i++)
                    {
                        if (random.NextDouble() < mutationRate)
                        {
                            if (random.Next(2) == 0)
                            {
                                neuron.dendrites[i].Weight *= (random.NextDouble() * (1.5 - .5)) + .5;//scale weight
                            }
                            else
                            {
                                neuron.dendrites[i].Weight *= -1;
                            }
                        }
                    }

                    //Mutate the Bias
                    if (random.NextDouble() < mutationRate)
                    {
                        if (random.Next(2) == 0)
                        {
                            neuron.bias *= (random.NextDouble() * (1.5 - .5)) + .5; //scale weight
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

                //Either copy from 0->cutPoint or cutPoint->Neurons.Length from the winner based on the flip variable
                for (int j = (flip ? 0 : cutPoint); j < (flip ? cutPoint : winLayer.Neurons.Length); j++)
                {
                    Neuron winNeuron = winLayer.Neurons[j];
                    Neuron childNeuron = childLayer.Neurons[j];

                    winNeuron.dendrites.CopyTo(childNeuron.dendrites, 0);
                    childNeuron.bias = winNeuron.bias;
                }
            }
        }

        //This function assumes all the fitness values have already been calculated
        public void Train((NeuralNetwork net, double fitness)[] population, Random random, double mutationRate)
        {
            Array.Sort(population, (a, b) => b.fitness.CompareTo(a.fitness));

            int start = (int)(population.Length * 0.1);
            int end = (int)(population.Length * 0.9);

            //Notice that this process is only called on networks in the middle 80% of the array
            for (int i = start; i < end; i++)
            {
                Crossover(population[random.Next(start)].net, population[i].net, random);
                Mutate(population[i].net, random, mutationRate);
            }

            //Removes the worst performing networks
            for (int i = end; i < population.Length; i++)
            {
                population[i].net.Randomize(random);
            }
        }

    }
}
