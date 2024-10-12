namespace NeuralNetworks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            HillClimber climber = new HillClimber("test", random);

            Console.WriteLine(climber.Run());
        }
    }
}
