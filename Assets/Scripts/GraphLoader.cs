using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphLoader : MonoBehaviour
{
    // Declare private variables
    private string saveFile;
    private Graph graph;
    private Draw draw;

    private void Start()
    {
        saveFile = Path.Combine(Application.persistentDataPath, "graph.json");

        graph = GameObject.Find("Spawner").GetComponent<Graph>();
        draw = GameObject.Find("Spawner").GetComponent<Draw>();
    }

    private void Update()
    {
        // Save graph if is pressed
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveGraph();
        }

        // Load graph if key is pressed
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGraph();
        }

        // Clear graph if key is pressed
        if (Input.GetKeyDown(KeyCode.C))
        {
            ResetGraph();
        }
    }

    // LoadGraph loads the saved graph into the scene
    public void LoadGraph()
    {
        Debug.Log("Loading graph");

        // Check if file exists
        if (!File.Exists(saveFile))
        {
            return;
        }

        // Read file and deserialze
        GraphData graphData = LoadFromFile();

        // If no data was read, return with an error
        if (graphData == null)
        {
            Debug.LogError("Data loaded was null");
            return;
        }

        // Delete current graph
        ResetGraph();

        // Apply loaded graph
        LoadGraphFromGraphData(graphData);

        Debug.Log("Successfully loaded graph");
    }

    // SaveGraph saves the current graph in the scene
    public void SaveGraph()
    {
        Debug.Log("Saving graph");

        try
        {
            // Make sure directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(saveFile));

            GraphData graphData = ConvertToGraphData();

            // Serialize to json
            string graphAsJson = JsonUtility.ToJson(graphData, true);

            // Save to file
            using (FileStream stream = new FileStream(saveFile, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(graphAsJson);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured while saving graph to: " + saveFile + "\n" + e);
        }

        Debug.Log("Successfully saved graph");
    }

    // LoadGraphFromGraphData takes in GraphData and loads it into the graph
    private void LoadGraphFromGraphData(GraphData graphData)
    {
        // Recreate all vertices
        foreach (Vector3 vertex in graphData.vertexPos)
        {
            Vector2 pos = new Vector3(vertex.x, vertex.y);
            graph.CreateVertex(pos, draw.baseVertexColor);
        }

        // Recreate all connections
        foreach (Vector3 connection in graphData.connections)
        {
            // Find the two vertices the connection is connecting
            Vertex v1 = graph.vertices[(int)connection.x];
            Vertex v2 = graph.vertices[(int)connection.y];

            graph.CreateConnection(v1, v2, connection.z);
        }

        // Load start vertex if saved
        if (graphData.startVertex >= 0)
        {
            graph.vertices[graphData.startVertex].SetColor(draw.startColor);
            graph.startVertex = graph.vertices[graphData.startVertex];
        }

        // Load end vertex if saved
        if (graphData.endVertex >= 0)
        {
            graph.vertices[graphData.endVertex].SetColor(draw.endColor);
            graph.endVertex = graph.vertices[graphData.endVertex];
        }
    }

    // LoadFromFile reads the file and deserializes to GraphData
    private GraphData LoadFromFile()
    {
        try
        {
            GraphData graphData;
            string data = "";

            // Load graph from file
            using (FileStream stream = new FileStream(saveFile, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    data = reader.ReadToEnd();
                }
            }

            // Deserialize graph
            graphData = JsonUtility.FromJson<GraphData>(data);
            return graphData;
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured while loading graph from: " + saveFile + "\n" + e);
            return null;
        }
    }

    /*
     * ConvertToGraphData creates a new GraphData, which only keeps necessary information
     * 
     * List<Vector3> vertexPos:   Vector3(x, y, z)
     * List<Vector3> connections: Vector3(index of vertex1, index of vertex2, weight)
     * int startVertex:           index of starting vertex
     * int endVertex:             index of ending vertex
     */
    private GraphData ConvertToGraphData()
    {
        // Declare variables to save
        List<Vector3> vertexPos = new List<Vector3>();
        List<Vector3> connections = new List<Vector3>();
        int startVertex = -1;
        int endVertex = -1;

        // Hashset of all connections (makes sure each connection is only saved once)
        HashSet<Connection> hashedConnections = new HashSet<Connection>();

        // Only save pos for all vertices and start/end vertex
        for (int i = 0; i < graph.vertices.Count; i++)
        {
            // Get vertex
            Vertex vertex = graph.vertices[i];

            // Check for start or end vertex
            if (vertex == graph.startVertex)
            {
                startVertex = i;
            }
            else if (vertex == graph.endVertex)
            {
                endVertex = i;
            }

            // Save position of vertex
            vertexPos.Add(vertex.pos);

            // Add all connections to hashset (will not be added if it already exists)
            foreach (Connection connection in vertex.connections)
            {
                hashedConnections.Add(connection);
            }
        }

        // Only save necessary parts from connections (vertex1 index, vertex2 index, weight)
        foreach (Connection connection in hashedConnections)
        {
            // Save the two index of the vertices
            int i1 = -1;
            int i2 = -1;

            // Find the index of the vertices
            for (int i = 0; i < graph.vertices.Count; i++)
            {
                if (graph.vertices[i].connections.Contains(connection))
                {
                    // Change the one that hasn't been set
                    if (i1 < 0)
                    {
                        i1 = i;
                    }
                    else
                    {
                        i2 = i;
                    }
                }
            }

            // Save the connection
            connections.Add(new Vector3(i1, i2, connection.weight));
        }

        // Create GraphData and return it
        return new GraphData(vertexPos, connections, startVertex, endVertex);
    }

    // ResetGraph resets all variables the graph depends on
    private void ResetGraph()
    {
        draw.ResetMarkedVertex();
        while (graph.vertices.Count > 0)
        {
            draw.DeleteVertex(graph.vertices[0]);
        }
        graph.startVertex = null;
        graph.endVertex = null;
    }
}
