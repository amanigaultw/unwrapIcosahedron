using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Icosahedron : MonoBehaviour
{
    [SerializeField, Range(0, 5)] // too many triangles (>65,535) for a single mesh past 5 subdivisions
    int subdivisionCount = 0;

    private Mesh mesh3D;
    private Mesh mesh2D;

    bool flatten = false;
    private float fraction = 0;
    private float speed = .5f;

    void Start()
    {
        buildMeshes();
        GetComponent<MeshFilter>().mesh = mesh2D;
    }

    private void Update()
    {
        unwrapAnimation();
    }

    private void buildMeshes()
    {
        List<Vector3> vertices = initialVertices();
        List<Vector3> flatVertices = initialFlatVertices();
        List<Triangle> triangles = initialTriangles(vertices);
        List<Triangle> flatTriangles = initialTriangles(flatVertices);

        for (int i = 0; i < subdivisionCount; i++)
        {
            subdivide(ref vertices, ref triangles, true);
            subdivide(ref flatVertices, ref flatTriangles, false);
        }

        mesh3D = getMesh(vertices, triangles);
        mesh2D = getMesh(flatVertices, flatTriangles);
    }

    private void unwrapAnimation()
    {
        fraction += Time.deltaTime * speed;

        if (fraction < 1)
        {
            Vector3[] meshVertices = GetComponent<MeshFilter>().mesh.vertices;
            if (meshVertices != null)
            {
                for (int i = 0; i < meshVertices.Length; i++)
                {
                    if (flatten)
                    {
                        meshVertices[i] = Vector3.Lerp(mesh3D.vertices[i], mesh2D.vertices[i], fraction);
                    }
                    else
                    {
                        meshVertices[i] = Vector3.Lerp(mesh2D.vertices[i], mesh3D.vertices[i], fraction);
                    }
                }
                GetComponent<MeshFilter>().mesh.vertices = meshVertices; // Reassign the modified vertices
            }
        }
        else if (fraction > 2)
        {
            flatten = !flatten;
            fraction = 0;
        }
    }

    private List<Vector3> initialVertices()
    {
        float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f; // ~1.618034

        List<Vector3> vertices = new List<Vector3>
        {

            //3D vertices
            new Vector3(-1, t, 0).normalized, //same vertice in 3D, different vertice in 2D
            new Vector3(-1, t, 0).normalized, //same vertice in 3D, different vertice in 2D
            new Vector3(-1, t, 0).normalized, //same vertice in 3D, different vertice in 2D
            new Vector3(-1, t, 0).normalized, //same vertice in 3D, different vertice in 2D
            new Vector3(-1, t, 0).normalized, //same vertice in 3D, different vertice in 2D

            new Vector3(1, t, 0).normalized, //same vertice in 3D, different vertice in 2D
            new Vector3(0, 1, t).normalized,
            new Vector3(-t, 0, 1).normalized,
            new Vector3(-t, 0, -1).normalized,
            new Vector3(0, 1, -t).normalized,
            new Vector3(1, t, 0).normalized, //same vertice in 3D, different vertice in 2D

            new Vector3(t, 0, 1).normalized, //same vertice in 3D, different vertice in 2D
            new Vector3(0, -1, t).normalized,
            new Vector3(-1, -t, 0).normalized,
            new Vector3(0, -1, -t).normalized,
            new Vector3(t, 0, -1).normalized,
            new Vector3(t, 0, 1).normalized, //same vertice in 3D, different vertice in 2D

            new Vector3(1, -t, 0).normalized, //same vertice in 3D, different vertice in 2D
            new Vector3(1, -t, 0).normalized, //same vertice in 3D, different vertice in 2D
            new Vector3(1, -t, 0).normalized, //same vertice in 3D, different vertice in 2D
            new Vector3(1, -t, 0).normalized, //same vertice in 3D, different vertice in 2D
            new Vector3(1, -t, 0).normalized //same vertice in 3D, different vertice in 2D
        };

        return vertices;
    }

    private List<Vector3> initialFlatVertices()
    {
        float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f; // ~1.618034
        float d = Vector3.Distance(new Vector3(-1, t, 0).normalized, new Vector3(1, t, 0).normalized); // compute the distance between two connected vertices
        float h = d * Mathf.Sqrt(3) / 2f; // compute the height, or the distance between the midpoint of two connected vertices and the third connected vertice

        List<Vector3> flatVertices = new List<Vector3>
        {

            //2D vertices
            new Vector3(d * .5f, h, 0), //same vertice in 3D, different vertice in 2D
            new Vector3(d * 1.5f, h, 0), //same vertice in 3D, different vertice in 2D
            new Vector3(d * 2.5f, h, 0), //same vertice in 3D, different vertice in 2D
            new Vector3(d * 3.5f, h, 0), //same vertice in 3D, different vertice in 2D
            new Vector3(d * 4.5f, h, 0), //same vertice in 3D, different vertice in 2D

            new Vector3(0, 0, 0), //same vertice in 3D, different vertice in 2D
            new Vector3(d, 0, 0),
            new Vector3(d * 2f, 0, 0),
            new Vector3(d * 3f, 0, 0),
            new Vector3(d * 4f, 0, 0),
            new Vector3(d * 5f, 0, 0), //same vertice in 3D, different vertice in 2D

            new Vector3(d * .5f, -h, 0), //same vertice in 3D, different vertice in 2D
            new Vector3(d * 1.5f, -h, 0),
            new Vector3(d * 2.5f, -h, 0),
            new Vector3(d * 3.5f, -h, 0),
            new Vector3(d * 4.5f, -h, 0),
            new Vector3(d * 5.5f, -h, 0), //same vertice in 3D, different vertice in 2D

            new Vector3(d, h * -2f, 0), //same vertice in 3D, different vertice in 2D
            new Vector3(d * 2f, h * -2f, 0), //same vertice in 3D, different vertice in 2D
            new Vector3(d * 3f, h * -2f, 0), //same vertice in 3D, different vertice in 2D
            new Vector3(d * 4f, h * -2f, 0), //same vertice in 3D, different vertice in 2D
            new Vector3(d * 5f, h * -2f, 0) //same vertice in 3D, different vertice in 2D
        };

        //add offset to 2D vertices
        for (int i = 0; i < flatVertices.Count; i++)
        {
            flatVertices[i] += new Vector3(-2.5f, .10f, 1f);
        }

        return flatVertices;
    }

    private List<Triangle> initialTriangles(List<Vector3> vertices)
    {
        List<Triangle> triangles = new List<Triangle>
        {
            new Triangle(new Vector3[] { vertices[5], vertices[0], vertices[6] }),
            new Triangle(new Vector3[] { vertices[6], vertices[1], vertices[7] }),
            new Triangle(new Vector3[] { vertices[7], vertices[2], vertices[8] }),
            new Triangle(new Vector3[] { vertices[8], vertices[3], vertices[9] }),
            new Triangle(new Vector3[] { vertices[9], vertices[4], vertices[10] }),

            new Triangle(new Vector3[] { vertices[5], vertices[6], vertices[11] }),
            new Triangle(new Vector3[] { vertices[6], vertices[7], vertices[12] }),
            new Triangle(new Vector3[] { vertices[7], vertices[8], vertices[13] }),
            new Triangle(new Vector3[] { vertices[8], vertices[9], vertices[14] }),
            new Triangle(new Vector3[] { vertices[9], vertices[10], vertices[15] }),

            new Triangle(new Vector3[] { vertices[11], vertices[6], vertices[12] }),
            new Triangle(new Vector3[] { vertices[12], vertices[7], vertices[13] }),
            new Triangle(new Vector3[] { vertices[13], vertices[8], vertices[14] }),
            new Triangle(new Vector3[] { vertices[14], vertices[9], vertices[15] }),
            new Triangle(new Vector3[] { vertices[15], vertices[10], vertices[16] }),

            new Triangle(new Vector3[] { vertices[11], vertices[12], vertices[17] }),
            new Triangle(new Vector3[] { vertices[12], vertices[13], vertices[18] }),
            new Triangle(new Vector3[] { vertices[13], vertices[14], vertices[19] }),
            new Triangle(new Vector3[] { vertices[14], vertices[15], vertices[20] }),
            new Triangle(new Vector3[] { vertices[15], vertices[16], vertices[21] })
        };

        return triangles;
    }

    private void subdivide(ref List<Vector3> vertices, ref List<Triangle> triangles, bool normalize = true)
    {
        List<Vector3> newVertices = new List<Vector3>();
        List<Triangle> newTriangles = new List<Triangle>();

        for (int i = 0; i < triangles.Count; i++)
        {
            Triangle triangle = triangles[i];

            List<Triangle> subTriangles = triangle.subdivide(normalize);
            List<Vector3> subVertices = triangle.getMidPoints(normalize).ToList();

            newTriangles = newTriangles.Concat(subTriangles).ToList();
            newVertices = newVertices.Concat(subVertices).ToList();
        }

        triangles = newTriangles;
        vertices = vertices.Concat(newVertices).ToList();
    }

    private Mesh getMesh(List<Vector3> vertices, List<Triangle> triangles)
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
        mesh.vertices = newVertices;
        mesh.RecalculateNormals();
        mesh.triangles = newTriangles;
        return mesh;
    }

}
