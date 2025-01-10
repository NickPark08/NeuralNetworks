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
            Random random = new Random();
            NeuralNetwork network = new NeuralNetwork(ActivationFunctions.BinaryStep, ErrorFunctions.MSE, random, [2, 2, 1]);
            double error = double.MaxValue;

            double[][] inputs = { [0, 0], [0, 1], [1, 0], [1, 1] };
            double[][] outputs = { [0], [1], [1], [0] };

            while(error > .5)
            {
                error = network.Train(inputs, outputs, .001);
                Console.WriteLine(error);
            }
            DisplayWinners();

            void DisplayWinners()
            {
                Console.WriteLine("---------");
                for (int i = 0; i < inputs.Length; i++)
                {
                    Console.WriteLine(network.Compute(inputs[i])[0]);
                }
                Console.WriteLine("---------");
            }
        }
    }
}