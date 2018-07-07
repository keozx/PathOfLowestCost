using System;
using System.Collections.Generic;
using System.Text;
using PathMobile.Models;

namespace PathMobile.Services
{
    public class PathResolver : IPathResolver
    {
        public Output Solve(int[,] input)
        {
            var output = new Output();
            int columns = input.GetLength(1);
            int rows = input.GetLength(0);

            for (int y = 0; y < input.GetLength(0); y++)
            {              
                for (int x = 0; x < input.GetLength(1); x++)
                {
                    
                    if (x == columns)
                    {
                        return output;
                    }
                    int next = GetNext(input, rows, y, x);
                    output.Cost += next;
                    output.Path.Add(next);
                    if (output.Cost > 50)
                    {
                        output.Solved = false;
                        return output;
                    }

                }
            }
            return output;
        }

        int GetNext(int[,] matrix, int Xmax, int Y, int X)
        {
            int numUp = matrix[Y == 0 ? Xmax : Y - 1, X + 1];
            int numMid = matrix[Y, X + 1];
            int numDown = matrix[Y == Xmax ? 0 : Y + 1, X + 1];
            return FindLowest(numUp, numMid, numDown);
        }

        int FindLowest(int up, int mid, int down)
        {
            if(mid < up)
            {
                if(mid < down)
                {
                    return mid;
                }
                else if(down < up)
                {
                    return down;
                }
            }
            return up;
        }
    }
}
