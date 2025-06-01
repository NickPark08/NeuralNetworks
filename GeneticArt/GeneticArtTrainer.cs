using System;
using System.Drawing;

public class GeneticArtTrainer
{
    public TriangleArt[] Population;
    private Random Rand = new Random();

    public GeneticArtTrainer(Bitmap original, int maxTriangles, int populationSize)
    {
        Population = new TriangleArt[populationSize];
        for (int i = 0; i < populationSize; i++)
        {
            Population[i] = new TriangleArt(maxTriangles, original);
        }
    }

    public double Train()
    {
        double bestError = double.MaxValue;
        int bestIndex = 0;

        for (int i = 1; i < Population.Length; i++)
        {
            Population[0].CopyTo(Population[i]);
            Population[i].Mutate(Rand);
            double error = Population[i].GetError();
            if (error < bestError)
            {
                bestError = error;
                bestIndex = i;
            }
        }

        if (bestIndex != 0)
        {
            var temp = Population[0];
            Population[0] = Population[bestIndex];
            Population[bestIndex] = temp;
        }

        return bestError;
    }

    public double ParallelTrain(Random rand)
    {
        double bestError = double.MaxValue;
        int bestIndex = 0;

        // Create a thread-local Random generator for thread safety
        //ThreadLocal<Random> threadLocalRand = new ThreadLocal<Random>(() =>
        //{
        //    return new Random(Guid.NewGuid().GetHashCode());
        //});

        double[] errors = new double[Population.Length];

        // Copy Population[0] to all others before mutating in parallel
        Parallel.For(1, Population.Length, i =>
        {
            //var rand = threadLocalRand.Value;

            // Copy base solution to this individual
            Population[0].CopyTo(Population[i]);

            // Mutate this individual
            Population[i].Mutate(rand);

            // Compute error
            double error = Population[i].GetError();

            errors[i] = error;
        });

        // Find best error and index in main thread
        for (int i = 1; i < Population.Length; i++)
        {
            if (errors[i] < bestError)
            {
                bestError = errors[i];
                bestIndex = i;
            }
        }

        // Swap best with index 0 if needed
        if (bestIndex != 0)
        {
            var temp = Population[0];
            Population[0] = Population[bestIndex];
            Population[bestIndex] = temp;
        }

        return bestError;
    }

    public Bitmap GetBestImage(int width, int height)
    {
        return Population[0].DrawImage(width, height);
    }
}
