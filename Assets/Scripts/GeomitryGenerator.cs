using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
public class GeomitryGenerator : MonoBehaviour
{
    [SerializeField] ShapeGenerator shape2D;
    [Range(0, 64)]
    [SerializeField] int segmentCount = 8;

    [SerializeField] Transform[] controlPoints = new Transform[4];
    Vector3 GetPosition(int i) => controlPoints[i].position;
    Vector3 GetScale(int i) => controlPoints[i].localScale;


    Mesh mesh;

    private void Awake()
    {
        mesh = new Mesh();
        mesh.name = "Segment";
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private void Update()
    {
        GenerateMesh();
    }

    void GenerateMesh()
    {
        mesh.Clear();

        float vSpan = shape2D.CalculateVSpan();
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();


        for (int segment = 0; segment < segmentCount; segment++)
        {
            float t = segment / (segmentCount - 1f);
            OrientedPoint orientedpoint = getBezierPoint(t);

            for (int i = 0; i < shape2D.vertices.Count; i++)
            {
                vertices.Add(orientedpoint.LocalToWorldPosition(shape2D.vertices[i].point* new Vector3(orientedpoint.scale.z, orientedpoint.scale.y, orientedpoint.scale.x)));
                normals.Add(orientedpoint.LocalToWorldVector(shape2D.vertices[i].normal));
                uvs.Add(new Vector2(shape2D.vertices[i].u, t * GetLength() / vSpan));


            }
        }

        List<int> triangleIndices = new List<int>();
        for (int segment = 0; segment < segmentCount-1; segment++)
        {
            int rootIndex = segment * shape2D.VertexCount;
            int rootIndexNext = (segment+1) * shape2D.VertexCount;

            for (int line = 0; line < shape2D.LineCount; line+=2)
            {
                int lineIndexA = shape2D.lineIndices[line];
                int lineIndexB = shape2D.lineIndices[line+1];
                int currentA = rootIndex + lineIndexA;
                int currentB = rootIndex + lineIndexB;
                int nextA = rootIndexNext + lineIndexA;
                int nextB = rootIndexNext + lineIndexB;

                triangleIndices.Add(currentA);
                triangleIndices.Add(nextA);
                triangleIndices.Add(nextB);
                triangleIndices.Add(currentA);
                triangleIndices.Add(nextB);
                triangleIndices.Add(currentB);

            }

        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangleIndices,0);
        mesh.SetNormals(normals);
        mesh.SetUVs(0,uvs);

    }


    public void OnDrawGizmos()
    {
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawSphere(GetPosition(i), 0.1f);
        }

        Handles.DrawBezier(
            GetPosition(0), 
            GetPosition(3), 
            GetPosition(1), 
            GetPosition(2), 
            Color.blue, EditorGUIUtility.whiteTexture, 1f
            );

        Gizmos.color = Color.green;

    
        Gizmos.color = Color.white;


    }

    OrientedPoint getBezierPoint(float t) {
        Vector3 p0 = GetPosition(0);
        Vector3 p1 = GetPosition(1);
        Vector3 p2 = GetPosition(2);
        Vector3 p3 = GetPosition(3);

        Vector3 pA = Vector3.Lerp(p0, p1, t);
        Vector3 pB = Vector3.Lerp(p1, p2, t);
        Vector3 pC = Vector3.Lerp(p2, p3, t);

        Vector3 pD = Vector3.Lerp(pA, pB, t);
        Vector3 pE = Vector3.Lerp(pB, pC, t);

        Vector3 s0 = GetScale(0);
        Vector3 s1 = GetScale(1);
        Vector3 s2 = GetScale(2);
        Vector3 s3 = GetScale(3);

        Vector3 sA = Vector3.Lerp(s0, s1, t);
        Vector3 sB = Vector3.Lerp(s1, s2, t);
        Vector3 sC = Vector3.Lerp(s2, s3, t);

        Vector3 sD = Vector3.Lerp(sA, sB, t);
        Vector3 sE = Vector3.Lerp(sB, sC, t);

        Vector3 position = Vector3.Lerp(pD, pE, t);
        Vector3 scale = Vector3.Lerp(sD, sE, t);

        Vector3 tangent =  (pE - pD).normalized;

        return new OrientedPoint(position, tangent, scale);

    }

    float GetLength(int precision = 8)
    {
        Vector3[] points = new Vector3[precision];

        for (int i = 0; i < precision; i++)
        {
            float t = i / (precision - 1);
            points[i] = getBezierPoint(t).position;
        }

        float distance = 0;
        for (int i = 0; i < precision-1; i++)
        {
            Vector3 a = points[i];
            Vector3 b = points[i+1];
            distance += Vector3.Distance(a, b);
        }
        return distance;
    }

   

}
