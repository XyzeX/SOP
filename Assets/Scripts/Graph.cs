using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    // Declare public variables
    public int runs = 100000;
    public GameObject circlePrefab;
    public GameObject linePrefab;
    public GameObject weightTextPrefab;
    public List<Vertex> vertices = new List<Vertex>();
    public Vertex startVertex = null;
    public Vertex endVertex = null;
    public float? heuristicConst = null;

    // Declare algorithms
    Dijkstra dijkstra = new Dijkstra("Dijkstra");
    AStar aStar = new AStar("AStar");

    private bool pathfinding = false;

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
            if (pathfinding)
            {
                return;
            }

            pathfinding = true;

            // Make sure heuristic constant is loaded
            if (heuristicConst == null)
            {
                heuristicConst = float.PositiveInfinity;
                // Since it isn't loaded, it needs to be calculated
                foreach (Vertex vertex in vertices)
                {
                    foreach (Edge edge in vertex.edges)
                    {
                        float diff = edge.weight / edge.length;
                        if (diff < heuristicConst)
                        {
                            heuristicConst = diff;
                        }
                    }
                }
            }

            // Start pathfinding in a coroutine
            StartCoroutine(Run());
        }
    }

    // Run runs the all pathfinding in a coroutine
    private IEnumerator Run()
    {
        // Reset graphs guesses
        ResetGraph();

        // Send graph to djikstra
        dijkstra.SetGraph(vertices, startVertex, endVertex, (float)heuristicConst);
        aStar.SetGraph(vertices, startVertex, endVertex, (float)heuristicConst);

        yield return null;

        // Start pathfinding

        dijkstra.PathFind(runs);
        dijkstra.TraceBack(Color.white);

        yield return new WaitForSeconds(2f);

        aStar.PathFind(runs);
        aStar.TraceBack(Color.black);

        pathfinding = false;
    }

    // CreateVertex creates a vertex at a given position
    public void CreateVertex(Vector2 pos, Color color)
    {
        // Convert position from 2D to 3D with z = -1 to show vertices on top of edges
        Vector3 position = new Vector3(pos.x, pos.y, -1);

        // Create instance for the vertex, with the spawner as it's parent
        GameObject circleInstance = Instantiate(circlePrefab, transform);

        // Save vertex in list of vertices
        Vertex vertex = new Vertex(position, circleInstance);
        vertex.SetColor(color);
        vertices.Add(vertex);
    }

    // CreateEdge creates an edge between two vertices with a given weight
    public void CreateEdge(Vertex vertex1, Vertex vertex2, float weight)
    {
        // Create instance for the edge, with the spawner as it's parent
        GameObject lineInstance = Instantiate(linePrefab, transform);
        GameObject weightText = Instantiate(weightTextPrefab, GameObject.Find("EdgeWeights").transform);

        // Give edge information to the vertices
        vertex1.AddEdge(vertex2, weight, lineInstance, weightText);
    }

    private void ResetGraph()
    {
        foreach (Vertex vertex in vertices)
        {
            vertex.bestWeight = float.PositiveInfinity;

            if (vertex == startVertex)
            {
                vertex.bestWeight = float.PositiveInfinity;
                vertex.hCost = float.PositiveInfinity;
            }
        }
    }
}

// GraphData is used for storing only necessary information to recreate the graph
public class GraphData
{
    public List<Vector3> vertexPos;
    public List<Vector3> edges;
    public int startVertex;
    public int endVertex;

    public GraphData(List<Vector3> _vertexPos, List<Vector3> _edges, int _startVertex, int _endVertex)
    {
        vertexPos = _vertexPos;
        edges = _edges;
        startVertex = _startVertex;
        endVertex = _endVertex;
    }
}
