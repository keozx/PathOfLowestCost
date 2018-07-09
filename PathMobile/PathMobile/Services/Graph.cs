using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using PathMobile.Models;

namespace PathMobile.Services
{
    public class Graph : IPathResolver
    {
        static int INF = int.MaxValue;
        Dictionary<int, List<int>> _path = new Dictionary<int, List<int>>();

        private int V;
        private List<AdjListNode>[] adj;
        public Graph(int v)
        {
            V = v;
            adj = new List<AdjListNode>[V];
            for (int i = 0; i < v; ++i)
                adj[i] = new List<AdjListNode>();
        }
        public void AddEdge(int u, int v, int weight)
        {
            AdjListNode node = new AdjListNode(v, weight);
            adj[u].Add(node);// Add v to u's list
        }

        public void TopologicalSortUtil(int v, Boolean[] visited, Stack<int> stack)
        {
            // Mark the current node as visited.
            visited[v] = true;

            // Recur for all the vertices adjacent to this vertex
            foreach (var node in adj[v])
            {
                if (!visited[node.V])
                    TopologicalSortUtil(node.V, visited, stack);
            }
            // Push current vertex to stack which stores result
            stack.Push(v);
        }
        public Output FindShortest(int s)
        {
            Stack<int> vertexStack = new Stack<int>();
            int[] dist = new int[V];

            // Mark all the vertices as not visited
            Boolean[] visited = new Boolean[V];
            for (int i = 0; i < V; i++)
                visited[i] = false;

            // Call the recursive helper function to store Topological
            // Sort starting from all vertices one by one
            for (int i = 0; i < V; i++)
                if (visited[i] == false)
                    TopologicalSortUtil(i, visited, vertexStack);

            // Initialize distances to all vertices as infinite and
            // distance to source as 0
            for (int i = 0; i < V; i++)
                dist[i] = INF;
            dist[s] = 0;

            // Process vertices in topological order
            while (vertexStack.Count != 0)
            {
                // Get the next vertex from topological order
                int u = (int)vertexStack.Pop();

                // Update distances of all adjacent vertices
                if (dist[u] != INF)
                {

                    foreach (AdjListNode node in adj[u])
                    {
                        if (dist[node.V] > dist[u] + node.Weight)
                            dist[node.V] = dist[u] + node.Weight;                           
                    }

                }
            }

            // Print the calculated shortest distances
            for (int i = 0; i < V; i++)
            {
                if (dist[i] == INF)
                    Debug.WriteLine("INF ");
                else
                    Debug.WriteLine(dist[i] + " ");
            }

            return new Output();
        }
    }
}
