using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Pathfinding
{
    // Pathfinding data
    //public Heap<Vertex> unsearchedVertices;
    public HashSet<Vertex> searchedVertices;

    // Graph
    public Vertex startVertex;
    public Vertex endVertex;
    public List<Vertex> vertices;

    // Overritten by algorithm
    public virtual void Algorithm() { }
    public string name = "";

    // Pathfind starts finding the shortest path
    public void PathFind(int runs)
    {
        int remaining = runs;

        // Copy vertices for reset
        List<Vertex> savedVertices = vertices;
        Vertex savedStartVertex = startVertex;
        Vertex savedEndVertex = endVertex;

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
    }

    // Save a copy of the graph
    public void SetGraph(List<Vertex> _vertices, Vertex _startVertex, Vertex _endVertex)
    {
        vertices = _vertices;
        startVertex = _startVertex;
        endVertex = _endVertex;

        startVertex.bestWeight = 0;

        //unsearchedVertices = new Heap<Vertex>(vertices.Count);
        searchedVertices = new HashSet<Vertex>();

        //unsearchedVertices.Add(startVertex);
    }

    // TraceBack walks through the solution, and colors the fastet path
    public void TraceBack()
    {
        float totalDistance = 0f;

        // Start at the end
        Vertex currentVertex = endVertex;

        // Keep painting until the start vertex is reached
        while (currentVertex != startVertex)
        {
            totalDistance += currentVertex.bestEdge.weight;
            currentVertex.bestEdge.SetColor(Color.white);
            currentVertex = currentVertex.bestEdge.GetOtherVertex(currentVertex);
        }

        UnityEngine.Debug.Log("Total Distance: " + totalDistance);
    }
}
