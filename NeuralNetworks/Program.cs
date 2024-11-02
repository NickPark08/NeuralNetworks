using System.Drawing;
using System.Net;

namespace NeuralNetworks
{
    internal class Program
    {

        static void Main(string[] args)
        {
            double ErrorFunc(double actual, double desired)
            {
                return Math.Pow(desired - actual, 2);
            }
            Random random = new Random();
            Func<double, double, double> errorFunc = ErrorFunc;
            double currentError = double.MaxValue;
            Perceptron perceptron = new Perceptron(3, .1, random, errorFunc);

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
                Console.WriteLine(Math.Round(val));
            }

            Console.WriteLine(currentError);

            //foreach (var val in perceptron.Compute(inputs))
            //{
            //    Console.WriteLine(val);
            //}
        }

    
    }
}