using TMPro;
using UnityEngine;

public class Connection
{
    // Declare public variables
    public float weight;
    public GameObject line;
    public GameObject weightText;
    public Vertex vertex1;
    public Vertex vertex2;

    // Constructer
    public Connection(Vertex _vertex1, Vertex _vertex2, float _weight, GameObject lineInstance, GameObject weightTextInstance)
    {
        // Save values
        vertex1 = _vertex1;
        vertex2 = _vertex2;
        line = lineInstance;
        weightText = weightTextInstance;
        SetWeight(_weight);

        float length = GetLineLength();

        // Correctly show the connection
        SetLineRotation(length);
        SetLinePosition();
        SetLineLength(length);

        // Orient weight text
        SetWeightTextTransform();
    }

    // GetOtherVertex returns the other vertex in the connection
    public Vertex GetOtherVertex(Vertex vertex)
    {
        if (vertex == vertex1)
        {
            return vertex2;
        }
        return vertex1;
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

    // Set weight
    public void SetWeight(float newWeight)
    {
        // Save new weight, and update visual text
        weight = newWeight;
        weightText.GetComponent<TMP_Text>().text = weight.ToString();
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
        line.transform.localScale = new Vector3(0.1f, length, 0.1f);
    }

    // GetLineLength calculates the distance between the two vertices in the connection
    private float GetLineLength()
    {
        // Calculate the square of the length using the pythagorean theorem
        float square = Mathf.Pow(vertex1.pos.x - vertex2.pos.x, 2) + Mathf.Pow(vertex1.pos.y - vertex2.pos.y, 2);

        // Return the length
        return Mathf.Sqrt(square);
    }

    // SetWeightTextTransform moves and rotates the text just above the line
    private void SetWeightTextTransform()
    {
        // Set rotation to line's rotation
        weightText.transform.rotation = line.transform.rotation;

        // Rotate 90 degrees to be oriented
        if (Mathf.Abs(weightText.transform.rotation.z + 90) < 270 && Mathf.Abs(weightText.transform.rotation.z + 90) > 90)
        {
            weightText.transform.Rotate(new Vector3(0, 0, -90));
        }
        else
        {
            weightText.transform.Rotate(new Vector3(0, 0, 90));
        }

        // Set text relative position
        weightText.transform.position = Camera.main.WorldToScreenPoint(line.transform.position);
        weightText.transform.Translate(new Vector3(0, 15, 0));
    }
}
