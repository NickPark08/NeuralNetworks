using System;
using System.Drawing;

public class GeneticArtTrainer
{
    public TriangleArt[] Population;
    private Random Rand = new Random();
    double bestError = double.MaxValue;

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

    //public double ParallelTrain(Random rand)
    //{
    //    double[] errors = new double[Population.Length];

    //    Parallel.For(1, Population.Length, (i) =>
    //    {
    //        Population[0].CopyTo(Population[i]);
    //        Population[i].Mutate(rand);
    //        errors[i] = Population[i].GetError();
    //    });

    //    int bestIndex = 0;
    //    for (int i = 1; i < errors.Length; i++)
    //    {
    //        if (errors[i] < bestError)
    //        {
    //            bestIndex = i;
    //            bestError = errors[i];
    //        }
    //    }
    //    TriangleArt temp = Population[0];
    //    Population[0] = Population[bestIndex];
    //    Population[bestIndex] = temp;
    //    return bestError;
    //}
    public double ParallelTrain(Random random)
    {
        double[] errors = new double[Population.Length];

        Parallel.For(1, Population.Length, i =>
        {
            Population[0].CopyTo(Population[i]);
            Population[i].Mutate(random);
            errors[i] = Population[i].GetError();
        });

        int bestIndex = 0;
        double error = double.MaxValue;

        for (int i = 1; i < errors.Length; i++)
        {
            if (errors[i] < error)
            {
                error = errors[i];
                bestIndex = i;
            }
        }

        if (error < bestError)
        {
            bestError = error;
        }

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
