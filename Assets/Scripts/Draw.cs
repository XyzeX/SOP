using UnityEngine;

public class Draw : MonoBehaviour
{
    // Declare public variables
    public Color baseVertexColor;

    // Declare private variables
    private Graph graph;
    private Vertex prevVertex;

    // Start is called before the first frame update
    private void Start()
    {
        // Keep a reference to the graph object
        graph = GameObject.Find("Spawner").GetComponent<Graph>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Check for left click to create a new vertex
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CreateVertexIfFree();
        }

        // Check for right click to create a new connection
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            MarkForConnection();
        }
    }

    private void MarkForConnection()
    {
        // Convert mouse position to world position
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        foreach (Vertex vertex in graph.vertices)
        {
            // Calculate distance to vertex
            float distance = GetDistance(vertex.pos, pos);

            // Check if mouse is within the circle, if not skip it
            if (distance >= vertex.instance.transform.localScale.x / 2)
            {
                continue;
            }
            
            // Handle the clicked vertex
            // Check if a previous vertex has been marked
            if (prevVertex == null)
            {
                // Mark the vertex that was clicked and save it
                vertex.SetColor(Color.red);
                prevVertex = vertex;
                return;
            }

            // Make sure it's not the same vertex that was clicked again
            if (prevVertex != vertex)
            {
                // Create connection between the two vertices with default weight 10
                graph.CreateConnection(prevVertex, vertex, 10f);
            }

            // Reset the marked vertex
            prevVertex.SetColor(baseVertexColor);
            prevVertex = null;
        }
    }

    // CreateVertexIfFree creates a vertex if the mouse is not too close to an existing vertex
    private void CreateVertexIfFree()
    {
        bool isSpotFree = true;

        // Convert mouse position to world position
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the distance between the mouse and all existing vertices
        foreach (Vertex vertex in graph.vertices)
        {
            // Calculate distance to vertex
            float distance = GetDistance(vertex.pos, pos);

            // If the mouse is too close to an existing vertex, do not create a new vertex
            if (distance < vertex.instance.transform.localScale.x * 1.25f)
            {
                isSpotFree = false;
                break;
            }
        }

        // If the position is available, create the new vertex
        if (isSpotFree)
        {
            graph.CreateVertex(pos, baseVertexColor);
        }
    }

    // GetDistance returns the distance between two positions
    private float GetDistance(Vector3 a, Vector3 b)
    {
        // Calculate distance using the pythagorean theorem
        float distance = Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2));
        return distance;
    }
}
