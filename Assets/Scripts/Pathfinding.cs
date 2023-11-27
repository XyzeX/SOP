using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Pathfinding
{
    // Pathfinding data
    public List<Vertex> unsearchedVertices;
    public HashSet<Vertex> searchedVertices;

    // Graph
    public Vertex startVertex;
    public Vertex endVertex;
    public List<Vertex> vertices;

    // Overritten by algorithm
    public virtual void Algorithm() { }
    public string name = "";

    // Pathfind starts finding the shortest path
    public IEnumerator Pathfind(int runs)
    {
        int remaining = runs;

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
        UnityEngine.Debug.Log(name + " found the path " + runs + " times in: " + timeTaken + "ms");

        // Show route in the end
        TraceBack();
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
            currentVertex.bestEdge.SetColor(Color.white);
            currentVertex = currentVertex.bestEdge.GetOtherVertex(currentVertex);
        }
    }
}
