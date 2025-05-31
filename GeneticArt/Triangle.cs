using GeneticArt;

using System;
using System.Drawing;

public class Triangle
{
    public Color Color { get; set; }
    public PointF[] Points { get; set; }

    public Triangle(PointF p0, PointF p1, PointF p2, Color color)
    {
        Points = [p0, p1, p2];
        Color = color;
    }

    public void Draw(Graphics gfx, float xCoeff, float yCoeff)
    {
        PointF[] scaledPoints = new PointF[3];
        for (int i = 0; i < 3; i++)
        {
            scaledPoints[i] = new PointF(Points[i].X * xCoeff, Points[i].Y * yCoeff);
        }
        var brush = new SolidBrush(Color);
        gfx.FillPolygon(brush, scaledPoints);
    }

    public void Mutate(Random rand)
    {
        // color
        if (rand.NextDouble() < 0.5)
        {
            int channel = rand.Next(4);
            int[] rgba = { Color.R, Color.G, Color.B, Color.A };
            rgba[channel] = Math.Clamp(rgba[channel] + rand.Next(-TriangleArtConstants.ColorMutationRange, TriangleArtConstants.ColorMutationRange + 1), 0, 255);
            Color = Color.FromArgb(rgba[3], rgba[0], rgba[1], rgba[2]);
        }
        // point
        else
        {
            int index = rand.Next(3);
            bool mutateX = rand.Next(2) == 0;
            if (mutateX)
            {
                Points[index].X += (float)rand.NextDouble() * (TriangleArtConstants.MaxPoint + TriangleArtConstants.MaxPoint) + (-TriangleArtConstants.MaxPoint);
            }
            else
            {
                Points[index].Y += (float)rand.NextDouble() * (TriangleArtConstants.MaxPoint + TriangleArtConstants.MaxPoint) + (-TriangleArtConstants.MaxPoint);
            }
        }
    }

    public Triangle Copy()
    {
        return new Triangle(new PointF(Points[0].X, Points[0].Y), new PointF(Points[1].X, Points[1].Y), new PointF(Points[2].X, Points[2].Y), Color);
    }

    public static Triangle RandomTriangle(Random rand)
    {
        PointF basePoint = new PointF((float)rand.NextDouble(), (float)rand.NextDouble());
        PointF[] points = new PointF[3];
        for (int i = 0; i < 3; i++)
        {
            points[i] = new PointF(
                Math.Clamp(basePoint.X + (float)(rand.NextDouble() * 0.1 - 0.05), 0, 1),
                Math.Clamp(basePoint.Y + (float)(rand.NextDouble() * 0.1 - 0.05), 0, 1)
            );
        }
        Color color = Color.FromArgb(rand.Next((int)TriangleArtConstants.MinAlpha, (int)TriangleArtConstants.MaxAlpha), rand.Next(256), rand.Next(256), rand.Next(256));
        return new Triangle(points[0], points[1], points[2], color);
    }
}
