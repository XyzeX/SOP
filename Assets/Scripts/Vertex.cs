using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    // Declare public variables
    public Vector3 pos;
    public GameObject instance;
    public List<Edge> edges = new List<Edge>();
    public Edge bestEdge;

    public float bestWeight = float.PositiveInfinity;

    // Constructer
    public Vertex(Vector3 _pos, GameObject circleInstance)
    {
        // Save values in class
        pos = _pos;
        instance = circleInstance;

        // Move circle instance to the given position
        instance.transform.position = pos;
    }

    // AddEdge creates a edge between two vertices given a weight and a new instance
    public void AddEdge(Vertex otherVertex, float weight, GameObject lineInstance, GameObject weightTextInstance)
    {
        // Create the edge between the two vertices and save it in the list
        Edge newEdge = new Edge(this, otherVertex, weight, lineInstance, weightTextInstance);
        edges.Add(newEdge);

        // Also save the edge on the other vertex
        otherVertex.edges.Add(newEdge);
    }

    // SetColor changes the color of the vertex
    public void SetColor(Color newColor)
    {
        // Apply new color to the sprite renderer
        instance.GetComponent<SpriteRenderer>().color = newColor;
    }

    // GetColor returns the current color of the vertex
    public Color GetColor()
    {
        // Read color from the sprite renderer
        return instance.GetComponent<SpriteRenderer>().color;
    }
}
