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
            GeneticGates network = new GeneticGates(0, 1, 1000, random);
            //double currentError = double.MaxValue;
            //double[][] inputs = [[0, 0, 0], [0, 1, 0], [1, 0, 0], [1, 1, 0], [0, 0, 1], [0, 1, 1], [1, 0, 1], [1, 1, 1]];
            double[][] inputs = [[0, 0], [0, 1], [1, 0], [1, 1]];

            double[] outputs = [0, 0, 0, 1];
            Population[] population = new Population[network.NetCount];
            NeuralNetwork winner = null;
            bool running = true;

            for (int i = 0; i < population.Length; i++)
            {
                population[i] = new Population(new NeuralNetwork(ActivationFunctions.BinaryStep, ErrorFunctions.MSE, random, [2, 3, 1]), double.MaxValue, network);
            }
            while (running)
            {
                for (int i = 0; i < population.Length; i++)
                {
                    population[i].Fitness = network.Fitness(population[i].Network, inputs, outputs)

                    ;
                }
                population = network.Train(population, random, 0.01);
                Console.WriteLine(population[0].Fitness);
                if (population[0].Fitness == outputs.Length)
                {
                    winner = population[0].Network;
                    running = false;
                }
            }

            DisplayWinner();

            void DisplayWinner()
            {
                Console.WriteLine("-");
                for (int i = 0; i < inputs.Length; i++)
                {
                    var test = winner.Compute(inputs[i]);
                    Console.WriteLine($"{inputs[i][0]}, {inputs[i][1]}: {test[0]} Expected: {outputs[i]}");
                }
                Console.WriteLine("-");
            }

        }


    }
}