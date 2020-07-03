using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingLines : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    void EnumerationCoordsPolygon()
    {
        Mesh mesh = model.GetComponent<MeshFilter>().mesh;
        Vector3[] verts = mesh.vertices;
        int[] triangles = mesh.triangles;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int i1 = (i + 0);
            int i2 = (i + 1);
            int i3 = (i + 2);

            Vector3 v1 = verts[triangles[i1]];
            Vector3 v2 = verts[triangles[i2]];
            Vector3 v3 = verts[triangles[i3]];
        }

    }
}
