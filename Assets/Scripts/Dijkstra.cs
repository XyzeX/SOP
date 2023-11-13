using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra
{
    // Pathfinding data
    private HashSet<Vertex> searchedVertices;
    private List<Vertex> unsearchedVertices;

    // Graph
    private Vertex startVertex;
    private Vertex endVertex;
    private List<Vertex> vertices;

    // Pathfind starts finding the shortest path
    public IEnumerator Pathfind()
    {
        while (unsearchedVertices.Count > 0)
        {
            Vertex vertex = unsearchedVertices[0];
            unsearchedVertices.RemoveAt(0);
            searchedVertices.Add(vertex);

            if (vertex == endVertex)
            {
                TraceBack();
                break;
            }

            foreach (Connection connection in vertex.connections)
            {
                Vertex otherVertex = connection.GetOtherVertex(vertex);

                float weightGuess = vertex.bestWeight + connection.weight;
                if (weightGuess <= otherVertex.bestWeight)
                {
                    otherVertex.bestWeight = weightGuess;
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

                Debug.Log(vertex.bestWeight);
                yield return new WaitForSeconds(.1f);
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
        }
    }
}
