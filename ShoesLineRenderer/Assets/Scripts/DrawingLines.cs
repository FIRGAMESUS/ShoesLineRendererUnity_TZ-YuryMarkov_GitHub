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
    public float dist;

    void Start()
    {
        dist = 0.5f;
        model = GameObject.Find("result");
        lineGeneratorPrefab = GameObject.Find("LineHolder");
        EnumerationCoordsPolygon();
        WriteMeshPoints(corners);

    }

    
    void Update()
    {
        
    }

    void EnumerationCoordsPolygon()
    {
        CornersToList();
        Mesh mesh = model.transform.GetChild(0).GetComponent<MeshFilter>().mesh;
        Vector3[] verts = mesh.vertices;
        int[] triangles = mesh.triangles;
        int step = 1;
        for (int i = 0; i < triangles.Length; i += step)
        {
            Vector3 v = verts[triangles[i]];
            if (CheckDist(corners, v, dist))
            {
                coords.Add(v);
                Debug.Log(v);
            }
        }

        WriteMeshPoints(coords);
    }

    void WriteMeshPoints(List<Vector3> points)
    {
        GameObject newLineGen = Instantiate(lineGeneratorPrefab);
        //newLineGen.transform.SetPositionAndRotation(model.transform.position, model.transform.rotation);
        //newLineGen.transform.localScale = model.transform.localScale;
        LineRenderer lRend = newLineGen.GetComponent<LineRenderer>();
        lRend.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            points[i] += model.transform.localPosition;
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
        corners.Add(new Vector3(7.06552431138356f, 3.152303227479417f, 0.5798208855947808f));
        corners.Add(new Vector3(3.520846622706104f, -7.299853716696669f, 0.852738011666962f));
        corners.Add(new Vector3(-11.26581889156483f, -2.161200109859713f, 1.420210236748185f));
        corners.Add(new Vector3(-7.721422806121932f, 8.215609420586972f, 0.7073115381738011f));

    }

    bool CheckDist(List<Vector3> pointsPlane, Vector3 p, float dist)
    {
        Vector3 t1 = pointsPlane[0];
        Vector3 t2 = pointsPlane[1];
        Vector3 t3 = pointsPlane[2];
        float a = t1.y * (t2.z - t3.z) + t2.y * (t3.z - t1.z) + t3.y * (t1.z - t2.z);
        float b = t1.z * (t2.x - t3.x) + t2.z * (t3.x - t1.x) + t3.z * (t1.x - t2.x);
        float c = t1.x * (t2.y - t3.y) + t2.x * (t3.y - t1.y) + t2.x * (t1.y - t2.y);
        float d = -1 * (t1.x * (t2.y * t3.z - t3.y * t2.z) + t2.x * (t3.y * t1.z - t1.y * t3.z) + t3.x * (t1.y * t2.z - t2.y * t1.z));

        float numerator = a * p.x + b * p.y + c * p.z + d;
        float denominator = (float)System.Math.Sqrt(a*a + b*b + c*c);
        float r = (numerator / denominator);

        if (System.Math.Abs(r - dist) <= dist * 0.5f) return true;
        else return false;
    }
}
