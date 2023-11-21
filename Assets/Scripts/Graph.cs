using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    // Declare public variables
    public GameObject circlePrefab;
    public GameObject linePrefab;
    public GameObject weightTextPrefab;
    public List<Vertex> vertices = new List<Vertex>();
    public Vertex startVertex = null;
    public Vertex endVertex = null;

    // Declare algorithms
    Dijkstra dijkstra = new Dijkstra();

    private void Update()
    {
        // Don't pathfind if the start and end vertices aren't set
        if (startVertex == null || endVertex == null)
        {
            return;
        }

        // Check for spacebar press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Reset graphs guesses
            ResetGraph();

            // Send graph to djikstra
            dijkstra.SetGraph(vertices, startVertex, endVertex);

            // Start pathfinding asynchronously
            Debug.Log("Starting Dijkstra");
            StartCoroutine(dijkstra.Pathfind());
        }
    }

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
        GameObject weightText = Instantiate(weightTextPrefab, GameObject.Find("ConnectionWeights").transform);

        // Give connection information to the vertices
        vertex1.AddConnection(vertex2, weight, lineInstance, weightText);
    }

    private void ResetGraph()
    {
        foreach (Vertex vertex in vertices)
        {
            vertex.bestWeight = float.PositiveInfinity;

            if (vertex == startVertex)
            {
                vertex.bestWeight = 0f;
            }
        }
    }
}

// GraphData is used for storing only necessary information to recreate the graph
public class GraphData
{
    public List<Vector3> vertexPos;
    public List<Vector3> connections;
    public int startVertex;
    public int endVertex;

    public GraphData(List<Vector3> _vertexPos, List<Vector3> _connections, int _startVertex, int _endVertex)
    {
        vertexPos = _vertexPos;
        connections = _connections;
        startVertex = _startVertex;
        endVertex = _endVertex;
    }
}
