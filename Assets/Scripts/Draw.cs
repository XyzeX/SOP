using TMPro;
using UnityEngine;
using System;

public class Draw : MonoBehaviour
{
    // Declare public variables
    public Color baseVertexColor;
    public Color markColor;
    public Color startColor;
    public Color endColor;
    public GameObject inputWindow;

    // Declare private variables
    private TMP_InputField inputField;
    private Graph graph;
    private Vertex prevVertex;
    private Connection markedConnection;
    private string numbers = "0123456789,";

    // Start is called before the first frame update
    private void Start()
    {
        // Get UI elements
        inputField = inputWindow.transform.Find("InputField (TMP)").GetComponent<TMP_InputField>();

        // Only accept numbers in inputField
        inputField.onValidateInput = (string text, int i, char newChar) =>
        {
            if (numbers.IndexOf(newChar) != -1)
            {
                // Return number
                return newChar;
            }
            else
            {
                // Not a number
                return '\0';
            }
        };

        // Hide UI from the beginning
        inputWindow.SetActive(false);

        // Keep a reference to the graph object
        graph = GameObject.Find("Spawner").GetComponent<Graph>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Don't do any drawing if window is open
        if (inputWindow.activeSelf)
        {
            // Shortcut to press OK
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnOK();
            }

            return;
        }

        // Check for left click to create a new vertex
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            HandleLeftClick();
        }

        // Check for right click to create a new connection
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            HandleRightClick();
            ResetMarkedVertex();
        }

        // Check for backspace to delete vertex
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete))
        {
            DeleteMark();
        }
    }

    // DeleteMark deletes marked vertex and its connections
    private void DeleteMark()
    {
        // Check if a vertex is marked
        if (prevVertex != null)
        {
            // Mark a connected vertex if it exists
            Vertex newPrevVertex = DeleteVertex(prevVertex);
            if (newPrevVertex != null)
            {
                MarkVertex(newPrevVertex);
            }
            else
            {
                prevVertex = null;
            }
        }
    }

    // Deletes the given vertex and it's connections
    public Vertex DeleteVertex(Vertex vertex)
    {
        Vertex newPrevVertex = null;

        foreach (Connection connection in vertex.connections)
        {
            Vertex otherVertex = connection.GetOtherVertex(vertex);
            newPrevVertex = otherVertex;

            // Delete connection for the other vertex
            otherVertex.connections.Remove(connection);

            // Delete weight number
            Destroy(connection.weightText);

            // Delete visual connection
            Destroy(connection.line);
        }

        // Check if vertex was start or end
        if (vertex == graph.startVertex)
        {
            graph.startVertex = null;
        }
        else if (vertex == graph.endVertex)
        {
            graph.endVertex = null;
        }

        // Delete visual vertex
        Destroy(vertex.instance);

        // Delete vertex from graph
        graph.vertices.Remove(vertex);

        return newPrevVertex;
    }

    // HandleRightClick finds the vertex to toggle
    private void HandleRightClick()
    {
        // Convert mouse position to world position
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        foreach (Vertex vertex in graph.vertices)
        {
            float distance = GetDistance(vertex.pos, pos);

            // Check if the user clicked on the vertex
            if (distance <= vertex.instance.transform.localScale.x / 2)
            {
                ToggleVertexColor(vertex);
                return;
            }
        }
    }
    
    // ToggleVertexColor changes a vertex to goal, start or normal vertex
    private void ToggleVertexColor(Vertex vertex)
    {
        // Get current color of the vertex
        Color color = vertex.GetColor();

        // Go from base color to start color, if start is taken set to end color
        if (color == baseVertexColor || color == markColor)
        {
            if (graph.startVertex == null)
            {
                vertex.SetColor(startColor);
                graph.startVertex = vertex;
            }
            else if (graph.endVertex == null)
            {
                vertex.SetColor(endColor);
                graph.endVertex = vertex;
            }
        }
        // Go from start color to end color, if end is taken set to base color
        else if (color == startColor)
        {
            graph.startVertex = null;
            if (graph.endVertex == null)
            {
                vertex.SetColor(endColor);
                graph.endVertex = vertex;
            }
            else
            {
                vertex.SetColor(baseVertexColor);
            }
        }
        // Go from end color to base color
        else if (color == endColor)
        {
            graph.endVertex = null;
            vertex.SetColor(baseVertexColor);
        }
    }

    // Marks vertices when clicked on, or creates new when clicking in empty space
    private void HandleLeftClick()
    {
        bool isSpotFree = true;

        // Convert mouse position to world position
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the distance between the mouse and all existing vertices
        foreach (Vertex vertex in graph.vertices)
        {
            // Calculate distance to vertex
            float distance = GetDistance(vertex.pos, pos);

            // Check if the user clicked on the vertex
            if (distance <= vertex.instance.transform.localScale.x / 2)
            {
                MarkForConnection(vertex);
                return;
            }
            // If the mouse is too close to an existing vertex, do not create a new vertex
            else if (distance < vertex.instance.transform.localScale.x * 1.25f)
            {
                isSpotFree = false;
            }
        }

        // If the position is available, create the new vertex
        if (isSpotFree)
        {
            graph.CreateVertex(pos, baseVertexColor);

            // If a vertex is marked, connect it to the new vertex
            if (prevVertex != null)
            {
                // Create connection and mark the new vertex
                Vertex newVertex = graph.vertices[graph.vertices.Count - 1];
                ConnectVertices(newVertex, prevVertex);
                ResetMarkedVertex();
                MarkVertex(newVertex);
            }
        }
    }

    // MarkForConnection handles checks for marking a vertex
    private void MarkForConnection(Vertex vertex)
    {
        // Check if a previous vertex has been marked
        if (prevVertex == null)
        {
            // Mark the vertex that was clicked and save it
            MarkVertex(vertex);
            return;
        }

        // Make sure it's not the same vertex that was clicked again
        if (prevVertex != vertex)
        {
            // Create connection between the two vertices
            ConnectVertices(vertex, prevVertex);
        }

        ResetMarkedVertex();
    }

    // ConnectVertices creates a connection between two vertices if it doesnt exist
    private void ConnectVertices(Vertex vertex1, Vertex vertex2)
    {
        // Check if connection already exists
        foreach (Connection connection in vertex1.connections)
        {
            // If the connection exists, exit
            if (connection.GetOtherVertex(vertex1) == vertex2)
            {
                return;
            }
        }

        // Find the starting weight
        float weight = 10 * Mathf.Sqrt(Mathf.Pow(vertex1.pos.x - vertex2.pos.x, 2) + Mathf.Pow(vertex1.pos.y - vertex2.pos.y, 2));

        // Get connection weight from user
        inputField.text = weight.ToString();
        inputWindow.SetActive(true);

        // Create connection between the two vertices with default weight 10
        graph.CreateConnection(vertex2, vertex1, weight);

        // Save connection for button click
        foreach(Connection connection in vertex1.connections)
        {
            if (connection.GetOtherVertex(vertex1) == vertex2)
            {
                markedConnection = connection;
                break;
            }
        }
    }

    // Called when user has finished inputting weight
    public void OnOK()
    {
        // Hide window
        inputWindow.SetActive(false);

        // Update weight
        float weight;
        float.TryParse(inputField.text, out weight);

        // Make sure mark isn't deleted (e.g. if graph was loaded while menu was active)
        try
        {
            markedConnection.SetWeight(weight);
        }
        catch (Exception e)
        {
            Debug.LogError("Mark no longer exists, and was therefore ignored\n" + e);
        }
    }

    // MarkVertex takes in a vertex and marks it
    private void MarkVertex(Vertex vertex)
    {
        vertex.SetColor(markColor);
        prevVertex = vertex;
    }

    // ResetMarkedVertex resets the marked vertex if it exists
    public void ResetMarkedVertex()
    {
        // Check if there is a vertex to reset
        if (prevVertex == null)
        {
            return;
        }

        // Reset the marked vertex
        if (graph.startVertex == prevVertex)
        {
            prevVertex.SetColor(startColor);
        }
        else if (graph.endVertex == prevVertex)
        {
            prevVertex.SetColor(endColor);
        }
        else
        {
            prevVertex.SetColor(baseVertexColor);
        }

        prevVertex = null;
    }

    // GetDistance returns the distance between two positions
    private float GetDistance(Vector3 a, Vector3 b)
    {
        // Calculate distance using the pythagorean theorem
        float distance = Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2));
        return distance;
    }
}
