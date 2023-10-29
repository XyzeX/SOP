using UnityEngine;

public class Connection
{
    // Declare public variables
    public float weight;

    // Declare private variables
    private Vertex vertex1;
    private Vertex vertex2;
    private GameObject line;

    // Constructer
    public Connection(Vertex _vertex1, Vertex _vertex2, float _weight, GameObject lineInstance)
    {
        // Save values in class
        vertex1 = _vertex1;
        vertex2 = _vertex2;
        weight = _weight;
        line = lineInstance;

        float length = GetLineLength();

        // Correctly show the connection
        SetLineRotation(length);
        SetLinePosition();
        SetLineLength(length);
    }

    // SetColor changes the color of the connection and both vertices to the new color
    public void SetColor(Color newColor)
    {
        // Apply new color to the sprite renderer on the connection instance
        line.GetComponent<SpriteRenderer>().color = newColor;

        // Apply new color to the vertices
        vertex1.SetColor(newColor);
        vertex2.SetColor(newColor);
    }

    // SetLineRotation rotates the rectangle to create a line between two vertices
    private void SetLineRotation(float length)
    {
        // Find the angle in degrees to rotate the rectangle to
        float degrees = Mathf.Asin((vertex1.pos.x - vertex2.pos.x) / length) * Mathf.Rad2Deg;

        // Create the rotation vector for the rotation
        Vector3 rotation = new Vector3(0, 0, degrees);

        // Invert rotation if rotated the wrong way
        if (vertex1.pos.y > vertex2.pos.y)
        {
            rotation *= -1;
        }

        // Rotate the rectangle to the correct orientation
        line.transform.Rotate(rotation);
    }

    // SetLinePosition moves the rectangle in between the vertices
    private void SetLinePosition()
    {
        // Find the point in the middle of the two vertices
        Vector3 position = (vertex1.pos + vertex2.pos) / 2;
        position.z = 0;

        // Move the rectangles center to the middle of the vertices
        line.transform.position = position;
    }

    // SetLineLength changes the scale of the rectangle to visually show the connection
    private void SetLineLength(float length)
    {
        line.transform.localScale = new Vector3(0.15f, length, 0.15f);
    }

    // GetLineLength calculates the distance between the two vertices in the connection
    private float GetLineLength()
    {
        // Calculate the square of the length using the pythagorean theorem
        float square = Mathf.Pow(vertex1.pos.x - vertex2.pos.x, 2) + Mathf.Pow(vertex1.pos.y - vertex2.pos.y, 2);

        // Return the length
        return Mathf.Sqrt(square);
    }
}
