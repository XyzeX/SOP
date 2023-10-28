using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    public Vector3 pos;
    public GameObject instance;

    public Vertex(Vector3 _pos, GameObject circleInstance)
    {
        pos = _pos;

        instance = circleInstance;
        instance.transform.position = pos;
    }

    public void SetColor(Color color)
    {
        instance.GetComponent<SpriteRenderer>().color = color;
    }
}
