using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticArt
{
    class TriangleArtConstants
    {
        public static readonly double AddChance = 0.2;
        public static readonly double RemoveChance = 0.05;
        public static readonly double MutateChance = 0.75;

        public static readonly float PointMutationRange = 0.05f;
        public static readonly int ColorMutationRange = 10;

        public static readonly float MinAlpha = 50;
        public static readonly float MaxAlpha = 200;

        public static readonly float MinPoint = 0f;
        public static readonly float MaxPoint = 1f;

    }
}
