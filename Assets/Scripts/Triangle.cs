using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle
{

    private Vector3[] vertices = new Vector3[3];

    public Triangle (Vector3[] vertices)
    {
        this.vertices = vertices;
    }

    public Vector3[] getVertices()
    {
        return vertices;
    }

    public List<Triangle> subdivide(bool normalize = true)
    {
        List<Triangle> triangles = new List<Triangle>();
        Vector3[] midPoints = getMidPoints(normalize);

        triangles.Add(new Triangle(new Vector3[] { vertices[0], midPoints[0], midPoints[1] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[1], midPoints[2], midPoints[0] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[2], midPoints[1], midPoints[2] }));
        triangles.Add(new Triangle(new Vector3[] { midPoints[0], midPoints[2], midPoints[1] }));

        return triangles;
    }

    public Vector3[] getMidPoints(bool normalize = true)
    {
        Vector3[] midPoints = new Vector3[3];

        midPoints[0] = ((vertices[0] + vertices[1]) / 2);
        midPoints[1] = ((vertices[0] + vertices[2]) / 2);
        midPoints[2] = ((vertices[1] + vertices[2]) / 2);

        if (normalize)
        {
            midPoints[0] = midPoints[0].normalized;
            midPoints[1] = midPoints[1].normalized;
            midPoints[2] = midPoints[2].normalized;
        }

        return midPoints;
    }

    public Vector3Int getIndices(List<Vector3> vertices)
    {
        return new Vector3Int (vertices.IndexOf(this.vertices[0]), vertices.IndexOf(this.vertices[1]), vertices.IndexOf(this.vertices[2]));
    }
}
