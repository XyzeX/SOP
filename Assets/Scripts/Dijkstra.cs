using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class Dijkstra
{
    // Pathfinding data
    private List<Vertex> unsearchedVertices;
    private HashSet<Vertex> searchedVertices;

    // Graph
    private Vertex startVertex;
    private Vertex endVertex;
    private List<Vertex> vertices;

    // Pathfind starts finding the shortest path
    public IEnumerator Pathfind()
    {
        int remaining = 100000;

        // Copy vertices for reset
        List<Vertex> savedVertices = vertices;
        Vertex savedStartVertex = startVertex;
        Vertex savedEndVertex = endVertex;

        yield return null;

        // Start a timer
        Stopwatch stopwatch = Stopwatch.StartNew();
        stopwatch.Start();

        // Run the algorithm <remaining> amount of times
        while (remaining > 0)
        {
            Algorithm();

            // Reset graph every time
            SetGraph(savedVertices, savedStartVertex, savedEndVertex);

            remaining--;
        }

        // Stop timer
        stopwatch.Stop();
        long timeTaken = stopwatch.ElapsedMilliseconds;
        UnityEngine.Debug.Log("Paths found in: " + timeTaken + "ms");

        // Show route in the end
        TraceBack();
    }

    private void Algorithm() {
        // Main loop, loops through all necessary vertices
        while (unsearchedVertices.Count > 0)
        {
            // Every loop, search from the first vertex in the list - the one which currently has the smalles total weight
            Vertex vertex = unsearchedVertices[0];

            // Remove it from the list of unsearched vertices, add it the the list of searched vertices instead
            unsearchedVertices.RemoveAt(0);
            searchedVertices.Add(vertex);

            // If the end vertex has been found, stop pathfinding
            if (vertex == endVertex)
            {
                break;
            }

            // Loop through all connections for the current vertex
            foreach (Connection connection in vertex.connections)
            {
                // Find the vertex that the connection is connected to
                Vertex otherVertex = connection.GetOtherVertex(vertex);

                // If the other vertex has already been searched, ignore it
                if (searchedVertices.Contains(otherVertex))
                {
                    continue;
                }

                // Calculate the weight using the current path
                float weightGuess = vertex.bestWeight + connection.weight;
                if (weightGuess <= otherVertex.bestWeight)
                {
                    // Keep the better total weight
                    otherVertex.bestWeight = weightGuess;

                    // Keep the connection for traceback
                    otherVertex.bestConnection = connection;
                }

                if (!unsearchedVertices.Contains(otherVertex))
                {
                    for (int i = 0; i < unsearchedVertices.Count; i++)
                    {
                        if (unsearchedVertices[i].bestWeight > weightGuess)
                        {
                            unsearchedVertices.Insert(i, otherVertex);
                            break;
                        }
                    }

                    if (!unsearchedVertices.Contains(otherVertex))
                    {
                        unsearchedVertices.Add(otherVertex);
                    }
                }
            }
        }
    }

    // Save a copy of the graph
    public void SetGraph(List<Vertex> _vertices, Vertex _startVertex, Vertex _endVertex)
    {
        vertices = _vertices;
        startVertex = _startVertex;
        endVertex = _endVertex;

        searchedVertices = new HashSet<Vertex>();
        unsearchedVertices = new List<Vertex>();

        startVertex.bestWeight = 0;
        unsearchedVertices.Add(startVertex);
    }

    // TraceBack walks through the solution, and colors the fastet path
    private void TraceBack()
    {
        // Start at the end
        Vertex currentVertex = endVertex;

        // Keep painting until the start vertex is reached
        while (currentVertex != startVertex)
        {
            currentVertex.bestConnection.SetColor(Color.white);
            currentVertex = currentVertex.bestConnection.GetOtherVertex(currentVertex);

            //yield return new WaitForSeconds(1f);
        }
    }
}
