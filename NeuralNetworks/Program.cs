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

            double testOutput = 0;

            //error super high
            //check both single and batch train for bugs

            while (currentError > .1)
            {
                //currentError = perceptron.TrainWithHillClimbing(inputs, outputs, currentError);
                currentError = perceptron.Train(inputs, outputs);
            }

            double[] finalOutputs = perceptron.Compute(inputs);

            foreach (var val in finalOutputs)
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