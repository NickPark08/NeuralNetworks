using System.Drawing;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;

namespace NeuralNetworks
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Random random = new Random(1);
            GeneticGates network = new GeneticGates(0, 1, 50, random);
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
                    population[i] = (network.networks[i], fitness);
                    if (fitness > 0)
                    {
                        winner = network.networks[i];
                        running = false;
                    }
                   
                    ;
                }
                network.Train(population, random, 0.1);
                winner = network.networks[0];
                DisplayWinner();
                //Console.WriteLine(winner.Compute(inputs[0])[0]);
            }

            void DisplayWinner()
            {
                Console.WriteLine("-");
                for (int i = 0; i < inputs.Length; i++)
                {
                    Console.WriteLine(winner.Compute(inputs[i])[0]);
                }
                Console.WriteLine("-");
            }
           
        }

    
    }
}