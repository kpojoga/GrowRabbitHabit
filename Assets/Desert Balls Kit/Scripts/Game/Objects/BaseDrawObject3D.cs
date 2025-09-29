using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// class for creating 3d objects along contours
public class BaseDrawObject3D : MonoBehaviour
{
    public Material material; // texture of the created object
    public float H = 1; // depth of this object
    public bool CreatePolygonCollider2D = false; // whether to create Polygon Collider 2D

    protected List<List<Vector3>> points = new List<List<Vector3>>(); // contour list
    protected Vector2 SizeREAL = new Vector2(1, 1); // texture size

    PolygonCollider2D polygonCollider2D;
    Mesh mesh = null;
    protected MeshRenderer meshRenderer;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector3> vertexNormals = new List<Vector3>();
    List<List<int>> ContoursPath = new List<List<int>>();



    public virtual void Draw()
    {
        CreateMesh();
    }

    // Method to browse the collider and launch makemesh
    void CreateMesh()
    {
        if (mesh == null || meshRenderer == null)
        {
            if (Application.isEditor && !Application.isPlaying)
            {
                MeshFilter mf;
                if (gameObject.GetComponent<MeshFilter>())
                    mf = gameObject.GetComponent<MeshFilter>();
                else
                    mf = gameObject.AddComponent<MeshFilter>();
                mesh = mf.mesh = new Mesh();
            }
            else
            {
                //do this in play mode
                if (gameObject.GetComponent<MeshFilter>())
                    mesh = gameObject.GetComponent<MeshFilter>().mesh;
                else
                    mesh = gameObject.AddComponent<MeshFilter>().mesh;
            }

            if (gameObject.GetComponent<MeshRenderer>())
                meshRenderer = gameObject.GetComponent<MeshRenderer>();
            else
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }
        meshRenderer.material = material;


        if (CreatePolygonCollider2D && polygonCollider2D == null)
        {
            if (gameObject.GetComponent<PolygonCollider2D>())
                polygonCollider2D = gameObject.GetComponent<PolygonCollider2D>();
            else
                polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
        }
        else if(!CreatePolygonCollider2D)
        {
            foreach(PolygonCollider2D pc in gameObject.GetComponents<PolygonCollider2D>())
            {
                if (Application.isEditor && !Application.isPlaying)
                {
                    DestroyImmediate(pc);
                }
                else
                {
                    Destroy(pc);
                }
            }
        }


        vertices.Clear();
        triangles.Clear();

        ContoursPath.Clear();


        if (CreatePolygonCollider2D)
        {
            polygonCollider2D.usedByComposite = true;
            polygonCollider2D.pathCount = points.Count;
            for (int i = 0; i < points.Count; i++)
            {
                polygonCollider2D.SetPath(i, points[i].Select(v => new Vector2(v.x, v.y)).ToArray());
            }
        }


        for (int i = 0; i < points.Count; i++)
        {
            ContoursPath.Add(new List<int>());

            if (Expantions.HasСlockwise(points[i]))
            {
                points[i].Reverse();
            }

            // browse all path point
            for (int j = 0; j < points[i].Count; j++)
            {
                AddPointsUV(points[i][j], ContoursPath.Last());
            }
            float ang_old = 0;
            int _id2 = -1;
            int _id3 = -1;
            for (int j = 0; j < points[i].Count; j++)
            {
                int _prev_j = (j == 0 ? points[i].Count : j) - 1;
                int _next_j = j == (points[i].Count-1) ? 0 : (j + 1);

                Vector3 _H = new Vector3(0, 0, H);
                float ang = Vector3.Angle(points[i][_prev_j] - points[i][j], points[i][_next_j] - points[i][j]);

                AddPointsUV(points[i][_prev_j], points[i][_prev_j] + _H);
                if (j == 0)
                {
                    _id3 = vertices.Count - 2;
                    _id2 = vertices.Count - 1;
                }
                if (j > 0 && ang_old > 135)
                {
                    int id0 = vertices.Count - 4;
                    int id1 = vertices.Count - 3;
                    int id3 = vertices.Count - 2;
                    int id2 = vertices.Count - 1;
                    MakeMesh(id0, id1, id2, id3);
                }
                if (ang <= 135)
                {
                    AddPointsUV(points[i][j], points[i][j] + _H);
                    int id0 = vertices.Count - 4;
                    int id1 = vertices.Count - 3;
                    int id3 = vertices.Count - 2;
                    int id2 = vertices.Count - 1;
                    MakeMesh(id0, id1, id2, id3);
                }
                if(j== points[i].Count - 1)
                {
                    int id0 = vertices.Count - 2;
                    int id1 = vertices.Count - 1;
                    int id3 = _id3;
                    int id2 = _id2;
                    MakeMesh(id0, id1, id2, id3);
                }
                ang_old = ang;
            }
        }

        foreach (List<int> cp in ContoursPath)
        {
            triangles.AddRange(Triangulate(cp));
        }

        Vector3 transform_pos = transform.position;


        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetUVs(0, vertices.Select(v => new Vector2((transform_pos.x + v.x) / SizeREAL.x, (transform_pos.y + v.y) / SizeREAL.y)).ToList());
        mesh.SetNormals(CalculateNormals());
        //mesh.RecalculateNormals();
    }

    //Method to generate 2 triangles mesh from the 4 points 0 1 2 3 and add it to the collider
    void MakeMesh(int id0, int id1, int id2, int id3)
    {
        triangles.Add(id0);
        triangles.Add(id2);
        triangles.Add(id1);
        triangles.Add(id0);
        triangles.Add(id3);
        triangles.Add(id2);
    }

    void AddPointsUV(Vector3 point0, Vector3 point1)
    {
        vertices.Add(point0);
        vertices.Add(point1);
    }

    void AddPointsUV(Vector3 point0, List<int> idVertices)
    {
        vertices.Add(point0);
        idVertices.Add(vertices.Count - 1);
    }

    // Updating normals
    List<Vector3> CalculateNormals()
    {
        vertexNormals.Clear();
        vertexNormals.AddRange(Enumerable.Repeat(new Vector3(0, 0, 0), vertices.Count));

        int triangleCount = triangles.Count / 3;
        for (int i = 0; i < triangleCount; i++)
        {
            int normalTriangleIndex = i * 3;
            int vertexIndexA = triangles[normalTriangleIndex];
            int vertexIndexB = triangles[normalTriangleIndex + 1];
            int vertexIndexC = triangles[normalTriangleIndex + 2];

            Vector3 triangleNormal = SurfaceNormalFormIndices(vertexIndexA, vertexIndexB, vertexIndexC);
            vertexNormals[vertexIndexA] += triangleNormal;
            vertexNormals[vertexIndexB] += triangleNormal;
            vertexNormals[vertexIndexC] += triangleNormal;
        }

        for (int i = 0; i < vertexNormals.Count; i++)
        {
            vertexNormals[i].Normalize();
        }

        return vertexNormals;
    }

    // Cross product of two vectors AB and AC
    Vector3 SurfaceNormalFormIndices(int indexA, int indexB, int indexC)
    {
        Vector3 pointA = vertices[indexA];
        Vector3 pointB = vertices[indexB];
        Vector3 pointC = vertices[indexC];

        Vector3 sideAB = pointB - pointA;
        Vector3 sideAC = pointC - pointA;

        return Vector3.Cross(sideAB, sideAC).normalized;
    }

    // Makes front side triangulation
    List<int> Triangulate(List<int> mPoints)
    {
        List<int> indices = new List<int>();

        int n = mPoints.Count;
        if (n < 3) return indices;

        int[] V = new int[n];
        if (Area(mPoints) > 0)
        {
            for (int v = 0; v < n; v++)
                V[v] = v;
        }
        else
        {
            for (int v = 0; v < n; v++)
                V[v] = (n - 1) - v;
        }

        int nv = n;
        int count = 2 * nv;
        for (int m = 0, v = nv - 1; nv > 2;)
        {
            if ((count--) <= 0)
                return indices;

            int u = v;
            if (nv <= u)
                u = 0;
            v = u + 1;
            if (nv <= v)
                v = 0;
            int w = v + 1;
            if (nv <= w)
                w = 0;

            if (Snip(u, v, w, nv, V, mPoints))
            {
                int a, b, c, s, t;
                a = mPoints[V[u]];
                b = mPoints[V[v]];
                c = mPoints[V[w]];
                indices.Add(a);
                indices.Add(b);
                indices.Add(c);
                m++;
                for (s = v, t = v + 1; t < nv; s++, t++)
                    V[s] = V[t];
                nv--;
                count = 2 * nv;
            }
        }

        indices.Reverse();
        return indices;
    }

    float Area(List<int> mPoints)
    {
        int n = mPoints.Count;
        float A = 0.0f;
        for (int p = n - 1, q = 0; q < n; p = q++)
        {
            Vector2 pval = vertices[mPoints[p]];
            Vector2 qval = vertices[mPoints[q]];
            A += pval.x * qval.y - qval.x * pval.y;
        }
        return (A * 0.5f);
    }

    bool Snip(int u, int v, int w, int n, int[] V, List<int> mPoints)
    {
        int p;
        Vector2 A = vertices[mPoints[V[u]]];
        Vector2 B = vertices[mPoints[V[v]]];
        Vector2 C = vertices[mPoints[V[w]]];
        if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x))))
            return false;
        for (p = 0; p < n; p++)
        {
            if ((p == u) || (p == v) || (p == w))
                continue;
            Vector2 P = vertices[mPoints[V[p]]];
            if (InsideTriangle(A, B, C, P))
                return false;
        }
        return true;
    }

    bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
    {
        float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
        float cCROSSap, bCROSScp, aCROSSbp;

        ax = C.x - B.x; ay = C.y - B.y;
        bx = A.x - C.x; by = A.y - C.y;
        cx = B.x - A.x; cy = B.y - A.y;
        apx = P.x - A.x; apy = P.y - A.y;
        bpx = P.x - B.x; bpy = P.y - B.y;
        cpx = P.x - C.x; cpy = P.y - C.y;

        aCROSSbp = ax * bpy - ay * bpx;
        cCROSSap = cx * apy - cy * apx;
        bCROSScp = bx * cpy - by * cpx;

        return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
    }
}
