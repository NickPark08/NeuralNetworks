using System.Drawing;

namespace NeuralNetworks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            HillClimber climber = new HillClimber("hello world", random);

            Console.WriteLine(climber.Run());
        }
    }
}
