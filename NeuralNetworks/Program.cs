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
            Perceptron perceptron = new Perceptron(3, .0005, random, ErrorFunctions.MSE, ActivationFunctions.Sigmoid);

            double[][] inputs = [[0, 0, 0], [0, 1, 0], [1, 0, 0], [1, 1, 0], [0, 0, 1], [0, 1, 1], [1, 0, 1], [1, 1, 1]];
            //double[][] inputs = [[0, 0], [0, 1], [1, 0], [1, 1]];

            double[] outputs = [0, 0, 0, 1, 0, 1, 1, 1];

            // learning rate decreases -> error should be able to go lower
            // bug in training, lower learning rate should mean lower error before overshoot
            while (currentError > .05)
            {
                currentError = perceptron.BatchTrain(inputs, outputs);
                Console.SetCursorPosition(0,0);
                Console.Write($"{currentError}              ");
            }

            double[] finalOutputs = perceptron.Compute(inputs);

            foreach (var val in finalOutputs)
            {
                Console.Write(perceptron.activationFunc.Function(val));
                //Console.WriteLine("      " + Math.Round(perceptron.activationFunc.Function(val)));

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