using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    // Declare public variables
    public GameObject circlePrefab;
    public GameObject linePrefab;
    public List<Vertex> vertices = new List<Vertex>();

    // CreateVertex creates a vertex at a given position
    public void CreateVertex(Vector2 pos, Color color)
    {
        // Convert position from 2D to 3D with z = -1 to show vertices on top of connections
        Vector3 position = new Vector3(pos.x, pos.y, -1);

        // Create instance for the vertex, with the spawner as it's parent
        GameObject circleInstance = Instantiate(circlePrefab, transform);

        // Save vertex in list of vertices
        Vertex vertex = new Vertex(position, circleInstance);
        vertex.SetColor(color);
        vertices.Add(vertex);
    }

    // CreateConnection creates a connection between two vertices with a given weight
    public void CreateConnection(Vertex vertex1, Vertex vertex2, float weight)
    {
        // Create instance for the connection, with the spawner as it's parent
        GameObject lineInstance = Instantiate(linePrefab, transform);

        // Give connection information to the vertices
        vertex1.AddConnection(vertex2, weight, lineInstance);
    }
}
