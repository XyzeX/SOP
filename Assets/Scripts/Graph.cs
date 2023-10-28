using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public GameObject circlePrefab;
    public GameObject linePrefab;
    public Vector2[] positions;
    public List<Vertex> vertices = new List<Vertex>();

    private bool run = false;

    private void Awake()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            CreateVertex(positions[i]);
        }
    }

    public void CreateVertex(Vector2 pos)
    {
        Vector3 position = new Vector3(pos.x, pos.y, -1);

        GameObject circleInstance = Instantiate(circlePrefab, transform);

        Vertex vertex = new Vertex(position, circleInstance);
        vertices.Add(vertex);
    }
}
