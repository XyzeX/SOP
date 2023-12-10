using TMPro;

public class Dijkstra : Pathfinding
{
    // Constructor
    public Dijkstra(string _name, TMP_Text _text)
    {
        name = _name;
        text = _text;
    }

    public override void Algorithm()
    {
        Heap<Vertex> unsearchedVertices = new Heap<Vertex>(vertices.Count);
        unsearchedVertices.Add(startVertex);

        // Main loop, loops through all necessary vertices
        while (unsearchedVertices.Count > 0)
        {
            // Every loop, search from the first vertex in the list
            // - the one which currently has the smallest total weight
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
                float weightGuess = vertex.bestWeight + edge.weight;

                // If we found a better path, go search the vertex
                // Or if the vertex doesn't have an exising guess, search it as well
                if (weightGuess < otherVertex.bestWeight || !unsearchedVertices.Contains(otherVertex))
                {
                    // Update costs
                    otherVertex.bestWeight = weightGuess;
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
}
