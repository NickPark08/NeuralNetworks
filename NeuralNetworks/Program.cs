using System.Drawing;
using System.Net;

namespace NeuralNetworks
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Random random = new Random();
            double currentError = double.MaxValue;
            NeuralNetwork network = new NeuralNetwork(ActivationFunctions.BinaryStep, ErrorFunctions.MSE, [2, 1, 1]);
            double[][] inputs = [[0, 0, 0], [0, 1, 0], [1, 0, 0], [1, 1, 0], [0, 0, 1], [0, 1, 1], [1, 0, 1], [1, 1, 1]];
            //double[][] inputs = [[0, 0], [0, 1], [1, 0], [1, 1]];

            double[] outputs = [0, 0, 0, 1, 0, 1, 1, 1];

        }

    
    }
}