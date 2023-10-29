using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    // Declare public variables
    public Vector3 pos;
    public GameObject instance;

    public List<Connection> connections = new List<Connection>();
    public Connection bestConnection;

    // Constructer
    public Vertex(Vector3 _pos, GameObject circleInstance)
    {
        // Save values in class
        pos = _pos;
        instance = circleInstance;

        // Move circle instance to the given position
        instance.transform.position = pos;
    }

    // AddConnection creates a connection between two vertices given a weight and a new instance
    public void AddConnection(Vertex otherVertex, float weight, GameObject lineInstance)
    {
        // Create the connection between the two vertices and save it in the list
        Connection newConnection = new Connection(this, otherVertex, weight, lineInstance);
        connections.Add(newConnection);

        // Also save the connection on the other vertex
        otherVertex.connections.Add(newConnection);
    }

    // SetColor changes the color of the vertex
    public void SetColor(Color newColor)
    {
        // Apply new color to the sprite renderer on the vertex instance
        instance.GetComponent<SpriteRenderer>().color = newColor;
    }
}
