using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator : MonoBehaviour
{
    
    [Range(0.01f,2f)]
    public float handleRadius = 0.1f;
    public bool closeShape = true;

    [Range(0f, 1f)]
    public float smoothness = 0f;

    [System.Serializable]
    public class Vertex
    {
        public Vector2 point;
        public Vector2 normal;
        public float u;
    }

    public List<Vertex> vertices;
    public List<int> lineIndices;

    public int VertexCount => vertices.Count;
    public int LineCount => lineIndices.Count;

   public Vertex CreateVertex()
   {
        return new Vertex();
   }

    public float CalculateVSpan()
    {
        float distance = 0;
        for (int i = 0; i < LineCount-1; i++)
        {
            Vector2 uA = vertices[lineIndices[i]].point;
            Vector2 uB = vertices[lineIndices[i + 1]].point;
            distance += Vector2.Distance(uA, uB);

        }

        return distance;
    }
}
