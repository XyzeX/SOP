using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : Pathfinding
{
    // Constructor
    public Dijkstra(string _name)
    {
        name = _name;
    }

    public override void Algorithm()
    {
        List<Vertex> unsearchedVertices = new List<Vertex>
        {
            startVertex
        };

        // Main loop, loops through all necessary vertices
        while (unsearchedVertices.Count > 0)
        {
            // Every loop, search from the first vertex in the list
            // - the one which currently has the smallest total weight
            Vertex vertex = unsearchedVertices[0];

            // Remove it from unsearched
            unsearchedVertices.RemoveAt(0);
            // Add it the the list of searched vertices instead
            searchedVertices.Add(vertex);

            // If the end vertex has been found, stop pathfinding
            if (vertex == endVertex)
            {
                break;
            }

            // Loop through all edges for the current vertex
            foreach (Edge edge in vertex.edges)
            {
                // Find the vertex that the edge is connected to
                Vertex otherVertex = edge.GetOtherVertex(vertex);

                // If the other vertex has already been searched, skip it
                if (searchedVertices.Contains(otherVertex))
                {
                    continue;
                }

                //otherVertex.SetColor(Color.red);

                // Calculate the weight using the current path
                float weightGuess = vertex.bestWeight + edge.weight;
                if (weightGuess <= otherVertex.bestWeight)
                {
                    // Keep the better total weight
                    otherVertex.bestWeight = weightGuess;

                    // Keep the edge for traceback
                    otherVertex.bestEdge = edge;
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
}
