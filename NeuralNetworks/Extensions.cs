using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public static class Extensions
    {
        public static int Add(this int a, int b) => a + b;

        public static bool SequenceEquals(this int[,] arr1, int[,] arr2)
        {
            for(int i = 0; i < arr1.GetLength(1); i++)
            {
                for(int j = 0; j < arr1.GetLength(0); j++)
                {
                    if (arr1[i, j] != arr2[i, j]) return false;
                }
            }
            return true;
        }

        public static bool TwoDContains (this int[,] arr1, int num)
        {
            for (int i = 0; i < arr1.GetLength(1); i++)
            {
                for (int j = 0; j < arr1.GetLength(0); j++)
                {
                    if (arr1[i, j] == num) return true;
                }
            }
            return false;
        }
    }
}
