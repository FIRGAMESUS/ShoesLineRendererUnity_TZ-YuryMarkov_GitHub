using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingLines : MonoBehaviour
{
    [SerializeField]
    private GameObject model;
    [SerializeField]
    private GameObject lineGeneratorPrefab;
    public List<Vector3> coords;

    void Start()
    {
        model = GameObject.Find("Result");
        lineGeneratorPrefab = GameObject.Find("LineHolder");
        EnumerationCoordsPolygon();
        WriteMeshPoints();
    }

    
    void Update()
    {
        
    }

    void EnumerationCoordsPolygon()
    {
        Mesh mesh = model.transform.GetChild(0).GetComponent<MeshFilter>().mesh;
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
            Debug.Log(v1 + " " + v2 + " " + v3);
            coords.Add(v1);
            //coords.Add(v2);
            //coords.Add(v3);
        }

    }

    void WriteMeshPoints()
    {
        GameObject newLineGen = Instantiate(lineGeneratorPrefab);
        LineRenderer lRend = newLineGen.GetComponent<LineRenderer>();
        lRend.positionCount = coords.Count;
        for (int i = 0; i < coords.Count; i++)
        {
            lRend.SetPosition(i, coords[i]);
        }
    }
}
