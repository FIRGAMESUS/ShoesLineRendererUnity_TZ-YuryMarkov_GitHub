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
    public List<Vector3> corners;

    void Start()
    {
        model = GameObject.Find("Result");
        lineGeneratorPrefab = GameObject.Find("LineHolder");
        EnumerationCoordsPolygon();
        CornersToList();
        WriteMeshPoints(coords);
        WriteMeshPoints(corners);
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
            //Debug.Log(v1 + " " + v2 + " " + v3);
            coords.Add(v1);
            //coords.Add(v2);
            //coords.Add(v3);
        }

    }

    void WriteMeshPoints(List<Vector3> points)
    {
        GameObject newLineGen = Instantiate(lineGeneratorPrefab);
        LineRenderer lRend = newLineGen.GetComponent<LineRenderer>();
        lRend.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            lRend.SetPosition(i, points[i]);
        }
    }

    void CornersToList()
    {
        /*
        temp 0 0 0
        bottomright 7.06552431138356 3.152303227479417 0.5798208855947808
        bottomleft 3.520846622706104 -7.299853716696669 0.852738011666962
        topleft -11.26581889156483 -2.161200109859713 1.420210236748185
        topright -7.721422806121932 8.215609420586972 0.7073115381738011
        */
        //corners.Add(new Vector3((float)0, (float)0, (float)0));
        corners.Add(new Vector3((float)7.06552431138356, (float)3.152303227479417, (float)0.5798208855947808));
        corners.Add(new Vector3((float)3.520846622706104, (float)-7.299853716696669, (float)0.852738011666962));
        corners.Add(new Vector3((float)-11.26581889156483, (float)-2.161200109859713, (float)1.420210236748185));
        corners.Add(new Vector3((float)-7.721422806121932, (float)8.215609420586972, (float)0.7073115381738011));

    }
}
