using GeneticArt;

using System;
using System.Collections.Generic;
using System.Drawing;

public class TriangleArt
{
    public List<Triangle> Triangles { get; set; }
    public Bitmap OriginalImage { get; set; }
    public int MaxTriangles { get; set; }

    public TriangleArt(int maxTriangles, Bitmap originalImage)
    {
        Triangles = new List<Triangle>();
        OriginalImage = originalImage;
        MaxTriangles = maxTriangles;
    }

    public void Mutate(Random rand)
    {
        double random = rand.NextDouble();
        if (random < TriangleArtConstants.AddChance)
        {
            Triangles.Add(Triangle.RandomTriangle(rand));
            if (Triangles.Count > MaxTriangles)
            {
                Triangles.RemoveAt(0);
            }
        }
        else if (random < TriangleArtConstants.AddChance + TriangleArtConstants.RemoveChance)
        {
            if (Triangles.Count > 0)
            {
                Triangles.RemoveAt(rand.Next(Triangles.Count));
            }
        }
        else if (Triangles.Count > 0)
        {
            Triangle t = Triangles[rand.Next(Triangles.Count)];
            t.Mutate(rand);
            if (t.Color.A == 0)
            {
                Triangles.Remove(t);
            }
        }
    }

    public Bitmap DrawImage(int width, int height)
    {
        Bitmap map = new Bitmap(width, height);
        using (Graphics gfx = Graphics.FromImage(map))
        {
            gfx.Clear(Color.White);
            foreach (Triangle t in Triangles)
            {
                t.Draw(gfx, width, height);
            }
        }
        return map;
    }

    public double GetError()
    {
        Bitmap current = DrawImage(OriginalImage.Width, OriginalImage.Height));
        double error = 0;
        for (int x = 0; x < current.Width; x++)
        {
            for (int y = 0; y < current.Height; y++)
            {
                Color c1 = current.GetPixel(x, y);
                Color c2 = OriginalImage.GetPixel(x, y);
                error += Math.Pow(c1.R - c2.R, 2) + Math.Pow(c1.G - c2.G, 2) + Math.Pow(c1.B - c2.B, 2);
            }
        }
        return error / (current.Width * current.Height);
    }

    public void CopyTo(TriangleArt other)
    {
        other.Triangles.Clear();
        foreach (var t in Triangles)
        {
            other.Triangles.Add(t.Copy());
        }
    }
}
