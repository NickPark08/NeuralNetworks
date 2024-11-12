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
            Perceptron perceptron = new Perceptron(3, .1, random, ErrorFunctions.MSE, ActivationFunctions.TanH);

            double[][] inputs = [[0, 0, 0], [0, 1, 0], [1, 0, 0], [1, 1, 0], [0, 0, 1], [0, 1, 1], [1, 0, 1], [1, 1, 1]];
            double[] outputs = [0, 0, 0, 1, 0, 1, 1, 1];
            double[] testInput = [1, 0];

            while (currentError > 0.085)
            {
                currentError = perceptron.TrainWithHillClimbing(inputs, outputs, currentError);
            }

            double[] finalOutputs = perceptron.Compute(inputs);

            foreach(var val in finalOutputs)
            {
                Console.Write(perceptron.activationFunc.Function(val));
                Console.WriteLine("      " + Math.Round(perceptron.activationFunc.Function(val)));

            }

            Console.WriteLine();
            Console.WriteLine(currentError);

            //foreach (var val in perceptron.Compute(inputs))
            //{
            //    Console.WriteLine(val);
            //}
        }

    
    }
}