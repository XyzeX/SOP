using System.Collections.Generic;
using UnityEngine;

public class Vertex : IHeapItem<Vertex>
{
    // Declare public variables
    public Vector3 pos;
    public GameObject instance;
    public List<Edge> edges = new List<Edge>();
    public Edge bestEdge;

    public float bestWeight = float.PositiveInfinity;
    public float hCost = float.PositiveInfinity;

    // Constructor
    public Vertex(Vector3 _pos, GameObject circleInstance)
    {
        // Save values in class
        pos = _pos;
        instance = circleInstance;

        // Move circle instance to the given position
        instance.transform.position = pos;
    }

    // Calculate fCost
    public float fCost()
    {
        return bestWeight + hCost;
    }



    // Implement Interface HeapIndex
    private int heapIndex;

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    // Implement IComparable (used in IHeapIndex)
    public int CompareTo(Vertex otherVertex)
    {
        // First of all compare the fCost
        int comparison = fCost().CompareTo(otherVertex.fCost());

        // If fCost is the same, let the hCost be the deciding factor
        if (comparison == 0)
        {
            comparison = hCost.CompareTo(otherVertex.hCost);
        }
        return -comparison;
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
