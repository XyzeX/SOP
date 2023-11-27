public class AStar : Pathfinding
{
    public AStar(string _name)
    {
        name = _name;
    }

    public override void Algorithm()
    {
        // Main loop, loops through all necessary vertices
        while (unsearchedVertices.Count > 0)
        {
            // Every loop, search from the first vertex in the list - the one which currently has the smalles total weight
            Vertex vertex = unsearchedVertices[0];

            // Remove it from the list of unsearched vertices, add it the the list of searched vertices instead
            unsearchedVertices.RemoveAt(0);
            searchedVertices.Add(vertex);
        }
    }
}
