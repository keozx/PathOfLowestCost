using System;
using System.Collections.Generic;
using System.Text;
using PathMobile.Models;

namespace PathMobile.Services
{
    public class PathResolver : IPathResolver
    {
        int y = 0;
        int x = 0;
        public Output Solve(int[,] input)
        {
            var output = new Output();
            int columns = input.GetLength(1);
            int rows = input.GetLength(0);

            //for (int y = 0; y < input.GetLength(0); y++)
            //{              
                for (x = 0; x < columns; x++)
                {
                    output.Path.Add(y + 1);

                    int next = GetNext(input, columns - 1, rows - 1);
                    output.Cost += next;
                    if (output.Cost > 50)
                    {
                        output.Solved = false;
                        return output;
                    }
                }
            //}
            return output;
        }

        int GetNext(int[,] matrix, int Xmax, int Ymax)
        {
            if (x == Xmax) return matrix[y, x];

            int numUp = matrix[y == 0 ? Ymax : y - 1, x + 1];
            int numMid = matrix[y, x + 1];
            int numDown = matrix[y == Ymax ? 0 : y + 1, x + 1];
          
            int lowest = FindLowestOf3(numUp, numMid, numDown);

            if (y > Ymax) y = 0;
            if (y < 0) y = Ymax;

            return lowest;
        }

        int FindLowestOf3(int up, int mid, int down)
        {
            if (mid < up && mid < down)
            {
                return mid;
            }
            else if (down < up)
            {
                y += 1;
                return down;
            }
            y -= 1;
            return up;
        }
    }
}
