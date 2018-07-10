using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using PathMobile.Helpers;
using PathMobile.Models;

namespace PathMobile.Services
{
    public class Graph : IPathResolver
    {
        static int INF = 9999;

        private List<NodeData> vertices;
        private int graphSize;
        private int[,] adjMatrix;
        public Graph(int size, int[,] matrix)
        {
            vertices = new List<NodeData>();
            graphSize = size;
            adjMatrix = new int[graphSize, graphSize];
            for (int i = 0; i < graphSize; i++)
            {
                adjMatrix[i, 0] = i;
            }

            vertices.Add(new NodeData("    ", -1));//no zero index to be used

            for(int x = 0; x < matrix.GetLength(0); x++)
            {
                for(int y = 0; y < matrix.GetLength(1); y++)
                {
                    var v = new NodeData($"V({matrix[y, x].ToString()})", y + 1);
                    vertices.Add(v);
                    if(x == matrix.GetLength(0) - 1)
                    {
                        v.IsEnd = true;
                    }
                }
            }

            //BuildEdges(matrix);
        }

        void BuildEdges(int[,] matrix)
        {
            int dist = 0;
            for (int y = 0; y < matrix.GetLength(1); y++)
            { 
                AddEdge(1, y + 1, matrix[y,0]);
            }

            int px = 1;
            int shiftY = matrix.GetLength(1);
            for (int x = 0; x < matrix.GetLength(0) - 1; x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    px++;

                    if (x - 1 <= 0)
                    {
                        dist = matrix[shiftY - 1, x + 1];
                        AddEdge(px, shiftY * (x + 1) + 4, dist);
                    }
                    else
                    {
                        dist = matrix[y - 1, x + 1];
                        AddEdge(px, shiftY * (x + 1) + 4, dist);
                    }

                    dist = matrix[y, x + 1];
                    AddEdge(px, shiftY * (x + 1) + 2,dist );

                    if (x + 1 >= matrix.GetLength(0) - 1)
                    {
                        dist = matrix[0, x + 1];
                        AddEdge(px, shiftY * (x + 1) + 3, dist);
                    }
                    else
                    {
                        dist = matrix[y + 1, x + 1];
                        AddEdge(px, shiftY * (x + 1) + 3, dist);
                    }
                }
            }
        }

        public NodeData FindShortest()
        {
            return vertices.Where(v => v.IsEnd).OrderBy(v => v.Distance).First() ;       
        }

        public void RunDijkstra()//runs dijkstras algorithm on the adjacency matrix
        {
            Debug.WriteLine("***********Dijkstra's Shortest Path***********");
            //Display();

            int[] distance = new int[graphSize];
            int[] previous = new int[graphSize];

            int source = 1;
            //for (int source = 1; source < firstVertices; source++)
            //{
                for (int i = 1; i < graphSize; i++)
                {
                    distance[i] = INF;
                    previous[i] = 0;
                }
                distance[source] = 0;

                PriorityQueue<int> pq = new PriorityQueue<int>();
                //enqueue the source
                pq.Enqueue(source, adjMatrix[source, source]);
                //insert all remaining vertices into the pq, could also be find v...
                for (int i = 1; i < graphSize; i++)
                {
                    for (int j = 1; j < graphSize; j++)
                    {
                        if (adjMatrix[i, j] > 0 && adjMatrix[i, j] != adjMatrix[source, source])
                        {
                            pq.Enqueue(i, adjMatrix[i, j]);
                        }
                    }
                }
                while (!pq.empty())
                {
                    int u = pq.dequeue_min();

                    for (int v = 1; v < graphSize; v++)//scan each row fully
                    {
                        if (adjMatrix[u, v] > 0)//if there is an adjacent node
                        {
                            int alt = distance[u] + adjMatrix[u, v];
                            if (alt < distance[v])
                            {
                                distance[v] = alt;
                                previous[v] = u;
                                pq.Enqueue(u, distance[v]);
                            }
                        }
                    }
                }
                //distance to 1..2..3..4..5..6 etc lie inside each index

                for (int i = 1; i < graphSize; i++)
                {
                    if (distance[i] == INF || distance[i] == 0)
                    {
                        Debug.WriteLine("Distance from {0} to {1}: --", source, i);
                    }
                    else
                    {
                        vertices[i-1].Distance = distance[i];
                        Debug.WriteLine("Distance from {0} to {1}: {2}", source, i, distance[i]);
                    }
                }
                for (int i = 1; i < graphSize; i++)
                {
                    printPath(previous, source, i, vertices[i-1]);
                    Debug.WriteLine(string.Empty);
                }
                Debug.WriteLine(string.Empty);
            //}
        }
        private void printPath(int[] path, int start, int end, NodeData node)
        {
            //prints a path, given a start and end, and an array that holds previous 
            //nodes visited
            Debug.WriteLine("Shortest path from {0} to {1}:", start, end);
            int temp = end;
            Stack<int> s = new Stack<int>();
            while (temp != start)
            {
                s.Push(temp);
                temp = path[temp];
                if (temp == 0)
                {
                    break;
                }
            }
            if (temp == 0 || start == end)
            {
                Debug.Write("No path");
                s.Clear();
                return;
            }
            Debug.Write($"{temp} -> ");//print source
            node.Path = new List<int>();
            while (s.Count != 0)
            {
                int v = s.Pop();
                Debug.Write($"{v} at {vertices[v-1].index} -> ");//print successive nodes to destination
                node.Path.Add(vertices[v - 1].index);
            }
        }
        public void AddEdge(int vertexA, int vertexB, int distance)
        {
            if (vertexA > 0 && vertexB > 0 && vertexA <= graphSize && vertexB <= graphSize)
            {
                adjMatrix[vertexA, vertexB] = distance;
            }
        }
        public void RemoveEdge(int vertexA, int vertexB)
        {
            if (vertexA > 0 && vertexB > 0 && vertexA <= graphSize && vertexB <= graphSize)
            {
                adjMatrix[vertexA, vertexB] = 0;
            }
        }
        public bool Adjacent(int vertexA, int vertexB)
        {   //checks whether two vertices are adjacent, returns true or false
            return (adjMatrix[vertexA, vertexB] > 0);
        }
        public int length(int vertex_u, int vertex_v)//returns a distance between 2 nodes
        {
            return adjMatrix[vertex_u, vertex_v];
        }
        public void Display() //displays the adjacency matrix
        {
            Debug.WriteLine("***********Adjacency Matrix Representation***********");
            Debug.WriteLine("Number of nodes: {0}\n", graphSize - 1);
            foreach (NodeData n in vertices)
            {
                Debug.Write($"{n.data}\t");
            }
            Debug.WriteLine("");//newline for the graph display
            for (int i = 1; i < graphSize; i++)
            {
                Debug.Write($"{vertices[adjMatrix[i, 0]].data}\t");
                for (int j = 1; j < graphSize; j++)
                {
                    Debug.Write($"{adjMatrix[i, j]}\t");
                }
                Debug.WriteLine("");
                Debug.WriteLine("");
            }
            Debug.WriteLine("Read the graph from left to right");
            Debug.WriteLine($"Example: Node A has an edge to Node C with distance: {length(1, 3)}");
        }
        private void DisplayNodeData(int v)//displays data/description for a node
        {
            Console.WriteLine(vertices[v].data);
        }
    }
}
