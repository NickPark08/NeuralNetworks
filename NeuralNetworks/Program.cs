using System.Drawing;
using System.Net;
using System.Transactions;

namespace NeuralNetworks
{
    internal class Program
    {

        static void Main(string[] args)
        {
            GeneticGates network = new GeneticGates(0, 1, 50);
            Random random = new Random();
            double currentError = double.MaxValue;
            //double[][] inputs = [[0, 0, 0], [0, 1, 0], [1, 0, 0], [1, 1, 0], [0, 0, 1], [0, 1, 1], [1, 0, 1], [1, 1, 1]];
            double[][] inputs = [[0, 0], [0, 1], [1, 0], [1, 1]];
            
            double[] outputs = [0, 0, 0, 1, 0, 1, 1, 1];
            (NeuralNetwork net, double fitness)[] population = new (NeuralNetwork, double)[network.networks.Length];
            NeuralNetwork winner = null;
            bool running = true;

            while (running)
            {
                for(int i = 0; i < population.Length; i++)
                {
                    double fitness = network.Fitness(network.networks[i], inputs, outputs);
                    if(fitness >= -.2)
                    {
                        winner = network.networks[i];
                        running = false;
                        break;
                    }
                    population[i] = (network.networks[i], fitness);
                }
                network.Train(population, random, 0.001);
            }

            Console.WriteLine(winner.Compute(inputs[0]));
        }

    
    }
}