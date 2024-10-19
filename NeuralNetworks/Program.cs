using System.Drawing;
using System.Net;

namespace NeuralNetworks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            double[] weights = { .75, -1.25 };
            double[][] inputs = [ [0,0], [.3, -.7], [1, 1], [-1, -1], [-.5, .5] ];
            Perceptron perceptron = new Perceptron(weights, .5);

            foreach (var val in perceptron.Compute(inputs))
            {
                Console.WriteLine(val);
            }
        }
    }
}