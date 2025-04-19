using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Icosahedron : MonoBehaviour
{
    [SerializeField, Range(0, 5)] // too many triangles for a single mesh past 5 subdivisions
    int subdivisionCount = 0;

    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector3> flatVertices = new List<Vector3>();
    private List<Triangle> triangles = new List<Triangle>();
    private List<Triangle> flatTriangles = new List<Triangle>();

    bool flatten = false;
    private float fraction = 0;
    private float speed = .5f;

    void Start()
    {
        initializeVertices();
        initializeFlatVertices();
        initializeTriangles(vertices, triangles);
        initializeTriangles(flatVertices, flatTriangles);
        for (int i = 0; i < subdivisionCount; i++)
        {
            subdivide();
            subdivideFlat();
        }
        drawFaces(flatVertices, flatTriangles);
        //drawVertices(flatVertices);
        //drawVertices(vertices);
    }

    private void Update()
    {
        fraction += Time.deltaTime * speed;
        if (fraction < 1)
        {
            Vector3[] vertices = GetComponent<MeshFilter>().mesh.vertices;
            if (vertices != null)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    if (!flatten)
                    {
                        vertices[i] = Vector3.Lerp(flatVertices[i], this.vertices[i], fraction);
                    }
                    else
                    {
                        vertices[i] = Vector3.Lerp(this.vertices[i], flatVertices[i], fraction);
                    }
                }
                GetComponent<MeshFilter>().mesh.vertices = vertices; // Reassign the modified vertices
            }
        }
        else if(fraction > 2)
        {
            flatten = !flatten;
            fraction = 0;
        }
    }

    private void initializeVertices()
    {
        float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f; // ~1.618034

        vertices.Add(new Vector3(-1, t, 0).normalized);
        vertices.Add(new Vector3(-1, t, 0).normalized);
        vertices.Add(new Vector3(-1, t, 0).normalized);
        vertices.Add(new Vector3(-1, t, 0).normalized);
        vertices.Add(new Vector3(-1, t, 0).normalized);

        vertices.Add(new Vector3(1, t, 0).normalized);
        vertices.Add(new Vector3(0, 1, t).normalized);
        vertices.Add(new Vector3(-t, 0, 1).normalized);
        vertices.Add(new Vector3(-t, 0, -1).normalized);
        vertices.Add(new Vector3(0, 1, -t).normalized);
        vertices.Add(new Vector3(1, t, 0).normalized);

        vertices.Add(new Vector3(t, 0, 1).normalized);
        vertices.Add(new Vector3(0, -1, t).normalized);
        vertices.Add(new Vector3(-1, -t, 0).normalized);
        vertices.Add(new Vector3(0, -1, -t).normalized);
        vertices.Add(new Vector3(t, 0, -1).normalized);
        vertices.Add(new Vector3(t, 0, 1).normalized);

        vertices.Add(new Vector3(1, -t, 0).normalized);
        vertices.Add(new Vector3(1, -t, 0).normalized);
        vertices.Add(new Vector3(1, -t, 0).normalized);
        vertices.Add(new Vector3(1, -t, 0).normalized);
        vertices.Add(new Vector3(1, -t, 0).normalized);
    }

    private void initializeFlatVertices(float offset = 3)
    {
        float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f; // ~1.618034
        float d = Vector3.Distance(new Vector3(-1, t, 0).normalized, new Vector3(1, t, 0).normalized);
        float h = d * Mathf.Sqrt(3) / 2f;

        flatVertices.Add(new Vector3(d * .5f - offset, h, 0));
        flatVertices.Add(new Vector3(d * 1.5f - offset, h, 0));
        flatVertices.Add(new Vector3(d * 2.5f - offset, h, 0));
        flatVertices.Add(new Vector3(d * 3.5f - offset, h, 0));
        flatVertices.Add(new Vector3(d * 4.5f - offset, h, 0));

        flatVertices.Add(new Vector3(0 - offset, 0, 0));
        flatVertices.Add(new Vector3(d - offset, 0, 0));
        flatVertices.Add(new Vector3(d * 2f - offset, 0, 0));
        flatVertices.Add(new Vector3(d * 3f - offset, 0, 0));
        flatVertices.Add(new Vector3(d * 4f - offset, 0, 0));
        flatVertices.Add(new Vector3(d * 5f - offset, 0, 0));

        flatVertices.Add(new Vector3(d * .5f - offset, -h, 0));
        flatVertices.Add(new Vector3(d * 1.5f - offset, -h, 0));
        flatVertices.Add(new Vector3(d * 2.5f - offset, -h, 0));
        flatVertices.Add(new Vector3(d * 3.5f - offset, -h, 0));
        flatVertices.Add(new Vector3(d * 4.5f - offset, -h, 0));
        flatVertices.Add(new Vector3(d * 5.5f - offset, -h, 0));

        flatVertices.Add(new Vector3(d - offset, h * -2f, 0));
        flatVertices.Add(new Vector3(d * 2f - offset, h * -2f, 0));
        flatVertices.Add(new Vector3(d * 3f - offset, h * -2f, 0));
        flatVertices.Add(new Vector3(d * 4f - offset, h * -2f, 0));
        flatVertices.Add(new Vector3(d * 5f - offset, h * -2f, 0));
    }

    private void initializeTriangles(List<Vector3> vertices, List<Triangle> triangles)
    {
        triangles.Add(new Triangle(new Vector3[] { vertices[5],  vertices[0],  vertices[6] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[6],  vertices[1],  vertices[7] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[7],  vertices[2],  vertices[8] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[8],  vertices[3],  vertices[9] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[9],  vertices[4],  vertices[10] }));

        triangles.Add(new Triangle(new Vector3[] { vertices[5],  vertices[6],  vertices[11] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[6],  vertices[7],  vertices[12] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[7],  vertices[8],  vertices[13] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[8],  vertices[9],  vertices[14] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[9],  vertices[10], vertices[15] }));

        triangles.Add(new Triangle(new Vector3[] { vertices[11], vertices[6],  vertices[12] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[12], vertices[7],  vertices[13] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[13], vertices[8],  vertices[14] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[14], vertices[9],  vertices[15] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[15], vertices[10], vertices[16] }));

        triangles.Add(new Triangle(new Vector3[] { vertices[11], vertices[12], vertices[17] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[12], vertices[13], vertices[18] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[13], vertices[14], vertices[19] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[14], vertices[15], vertices[20] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[15], vertices[16], vertices[21] }));
    }

    private void subdivide()
    {
        List<Vector3> newVertices = new List<Vector3>();
        List<Triangle> newTriangles = new List<Triangle>();

        for (int i = 0; i < triangles.Count; i++)
        {
            Triangle triangle = triangles[i];

            List<Triangle> subTriangles = triangle.subdivide();
            List<Vector3> subVertices = triangle.getMidPoints().ToList();

            newTriangles = newTriangles.Concat(subTriangles).ToList();
            newVertices = newVertices.Concat(subVertices).ToList();
        }

        triangles = newTriangles;
        vertices = vertices.Concat(newVertices).ToList();
    }

    private void subdivideFlat()
    {
        List<Vector3> newVertices = new List<Vector3>();
        List<Triangle> newTriangles = new List<Triangle>();

        for (int i = 0; i < flatTriangles.Count; i++)
        {
            Triangle triangle = flatTriangles[i];

            List<Triangle> subTriangles = triangle.subdivide(false);
            List<Vector3> subVertices = triangle.getMidPoints(false).ToList();

            newTriangles = newTriangles.Concat(subTriangles).ToList();
            newVertices = newVertices.Concat(subVertices).ToList();
        }

        flatTriangles = newTriangles;
        flatVertices = flatVertices.Concat(newVertices).ToList();
    }
    private void drawFaces(List<Vector3> vertices, List<Triangle> triangles)
    {
        List<int> triangleIndices = new List<int>();

        for (int i = 0; i < triangles.Count; i++)
        {
            Triangle triangle = triangles[i];
            Vector3Int indices = triangle.getIndices(vertices);
            triangleIndices.Add(indices[0]);
            triangleIndices.Add(indices[1]);
            triangleIndices.Add(indices[2]);
        }

        Vector3[] newVertices = vertices.ToArray();
        int[] newTriangles = triangleIndices.ToArray();

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = newVertices;
        mesh.RecalculateNormals();
        mesh.triangles = newTriangles;
    }

    private void drawVertices(List<Vector3> vertices)
    {
        int i = 0;
        foreach(Vector3 vertice in vertices)
        {
            GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            marker.transform.position = vertice;
            marker.transform.localScale = Vector3.one * .2f;
            marker.name = "vertice " + i + " " + vertice;
            i++;
        }
    }

}
