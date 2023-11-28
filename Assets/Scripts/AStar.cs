using System;
using UnityEngine;

public class AStar : Pathfinding
{
    // Constructor
    public AStar(string _name)
    {
        name = _name;
    }

    public override void Algorithm()
    {
        Heap<Vertex> unsearchedVertices = new Heap<Vertex>(vertices.Count);
        unsearchedVertices.Add(startVertex);

        // Main loop, loops through all necessary vertices
        while (unsearchedVertices.Count > 0)
        {
            // Every loop, search from the first vertex in the list
            // - the one which currently has the smallest fCost
            Vertex vertex = unsearchedVertices.RemoveFirstItem();

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

                // Calculate the weight using the current path
                float bestWeightGuess = vertex.bestWeight + edge.weight;

                if (bestWeightGuess < otherVertex.bestWeight || !unsearchedVertices.Contains(otherVertex))
                {
                    // Update costs
                    otherVertex.bestWeight = bestWeightGuess;
                    otherVertex.hCost = GetDistance(otherVertex, endVertex);
                    otherVertex.bestEdge = edge;

                    // Make sure the vertex is in the heap
                    if (!unsearchedVertices.Contains(otherVertex))
                    {
                        unsearchedVertices.Add(otherVertex);
                    }
                    // If it already is in the heap, move it if necessary
                    else
                    {
                        unsearchedVertices.UpdateItem(otherVertex);
                    }
                }
            }
        }
    }

    // GetDistance calculates a minimum distance to the other vertex
    private float GetDistance(Vertex v1, Vertex v2)
    {
        float distX = Mathf.Abs(v1.pos.x - v2.pos.x);
        float distY = Mathf.Abs(v1.pos.y - v2.pos.y);
        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX - 10 * (distY - distX);

        //return 10 * MathF.Sqrt(MathF.Pow(v1.pos.x + v2.pos.x, 2) + MathF.Pow(v1.pos.y + v2.pos.y, 2));
    }
}
