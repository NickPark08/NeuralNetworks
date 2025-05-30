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

    public Bitmap GetBestImage(int width, int height)
    {
        return Population[0].DrawImage(width, height);
    }
}
