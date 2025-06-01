using GeneticArt;

using System;
using System.Collections.Generic;
using System.Drawing;

public class TriangleArt
{
    public List<Triangle> Triangles { get; set; }
    public Bitmap OriginalImage { get; set; }
    public int MaxTriangles { get; set; }

    Bitmap map;
    Graphics gfx;
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
        if (gfx == null)
        {
            map = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            gfx = Graphics.FromImage(map);
        }
        gfx.Clear(Color.White);
        foreach (Triangle t in Triangles)
        {
            t.Draw(gfx, width, height);
        }

        return map;
    }

    //public double GetError()
    //{
    //    Bitmap current = DrawImage(OriginalImage.Width, OriginalImage.Height);
    //    double error = 0;
    //    for (int x = 0; x < current.Width; x++)
    //    {
    //        for (int y = 0; y < current.Height; y++)
    //        {


    //            Color color1 = current.GetPixel(x, y);
    //            Color color2 = OriginalImage.GetPixel(x, y);
    //            error += Math.Pow(color1.R - color2.R, 2) + Math.Pow(color1.G - color2.G, 2) + Math.Pow(color1.B - color2.B, 2);
    //        }
    //    }
    //    return error / (current.Width * current.Height);
    //}


    public double GetError()
    {
        Bitmap current = DrawImage(OriginalImage.Width, OriginalImage.Height);
        double error = 0;
        //var originalClone = (Bitmap)OriginalImage.Clone(*

        // Lock both bitmaps
        Rectangle rect = new Rectangle(0, 0, current.Width, current.Height);

        var bmpDataCurrent = current.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, current.PixelFormat);
        var bmpDataOriginal = OriginalImage.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, OriginalImage.PixelFormat);

        int strideCurrent = bmpDataCurrent.Stride;
        int strideOriginal = bmpDataOriginal.Stride;

        int bytesCurrent = Math.Abs(strideCurrent) * current.Height;
        int bytesOriginal = Math.Abs(strideOriginal) * OriginalImage.Height;

        byte[] rgbValuesCurrent = new byte[bytesCurrent];
        byte[] rgbValuesOriginal = new byte[bytesOriginal];

        System.Runtime.InteropServices.Marshal.Copy(bmpDataCurrent.Scan0, rgbValuesCurrent, 0, bytesCurrent);
        System.Runtime.InteropServices.Marshal.Copy(bmpDataOriginal.Scan0, rgbValuesOriginal, 0, bytesOriginal);

        int width = current.Width;
        int height = current.Height;

        for (int y = 0; y < height; y++)
        {
            int rowOffsetCurrent = y * strideCurrent;
            int rowOffsetOriginal = y * strideOriginal;

            for (int x = 0; x < width; x++)
            {
                int indexCurrent = rowOffsetCurrent + x * 3; // 3 bytes per pixel
                int indexOriginal = rowOffsetOriginal + x * 3;

                int blueDifference = rgbValuesCurrent[indexCurrent] - rgbValuesOriginal[indexOriginal];
                int greenDifference = rgbValuesCurrent[indexCurrent + 1] - rgbValuesOriginal[indexOriginal + 1];
                int redDifference = rgbValuesCurrent[indexCurrent + 2] - rgbValuesOriginal[indexOriginal + 2];

                error += redDifference * redDifference + greenDifference * greenDifference + blueDifference * blueDifference;
            }
        }

        current.UnlockBits(bmpDataCurrent);
        OriginalImage.UnlockBits(bmpDataOriginal);

        return error / (width * height);
    }




    public void CopyTo(TriangleArt other)
    {
        other.Triangles.Clear();
        foreach (var t in Triangles)
        {
            other.Triangles.Add(t.Copy());
        }
    }

    //public override string ToString()
    //{
    //    string data = "";
    //    foreach(var t in Triangles)
    //    {
    //        data += $"Color: {t.Color} -- Points: {t.Points[0]}, {t.Points[1]}, {t.Points[2]}";
    //    }

    //    return data;
    //}
}
