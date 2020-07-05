using System.Collections;
using System;
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
    float a, b, c, d;
    const float shortSideDefault = 210;
    const float longSideDefault = 297;
    const float distDefault = 10;
    float shortSide, longSide;

    void Start()
    {
        dist = 2f;
        model = GameObject.Find("result");
        lineGeneratorPrefab = GameObject.Find("LineHolder");
        SetCorners();
        CalculatePlaneABCD(corners);

        EnumerationCoordsPolygon();
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
        //Debug.Log(verts.Length);
        int step = 1;
        for (int i = 0; i < verts.Length; i += step)
        {
            Vector3 v = verts[i];
            if (CheckDist(v, dist))
            {
                coords.Add(v);
                //Debug.Log(v);
            }
        }

        WriteMeshPoints(coords);
    }

    void WriteMeshPoints(List<Vector3> points)
    {
        GameObject newLineGen = Instantiate(lineGeneratorPrefab);
        LineRenderer lRend = newLineGen.GetComponent<LineRenderer>();
        lRend.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            points[i] += model.transform.localPosition;
            lRend.SetPosition(i, points[i]);
        }
    }

    void SetCorners()
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
        CalculateSides(corners);
        
    }

    private void CalculateSides(List<Vector3> cp)
    {
        float shortSide1 = Mathf.Sqrt(Mathf.Pow(cp[0].x - cp[1].x, 2) + Mathf.Pow(cp[0].y - cp[1].y, 2) + Mathf.Pow(cp[0].y - cp[1].y, 2));
        float shortSide2 = Mathf.Sqrt(Mathf.Pow(cp[2].x - cp[3].x, 2) + Mathf.Pow(cp[2].y - cp[3].y, 2) + Mathf.Pow(cp[2].y - cp[3].y, 2));
        shortSide = Mathf.Abs((shortSide1 + shortSide2) / 2);
        //Debug.Log(shortSide);

        float longSide1 = Mathf.Sqrt(Mathf.Pow(cp[0].x - cp[3].x, 2) + Mathf.Pow(cp[0].y - cp[3].y, 2) + Mathf.Pow(cp[0].y - cp[3].y, 2));
        float longtSide2 = Mathf.Sqrt(Mathf.Pow(cp[2].x - cp[1].x, 2) + Mathf.Pow(cp[2].y - cp[1].y, 2) + Mathf.Pow(cp[2].y - cp[1].y, 2));
        longSide = (longSide1 + longtSide2) / 2;
        //Debug.Log(longSide);
        CalculateDist();
    }

    void CalculateDist()
    {
        float longRatio = longSideDefault / longSide;
        float shortRatio = shortSideDefault / shortSide;
        float averageMultiplier = (longRatio + shortRatio) / 2;
        //Debug.Log(averageMultiplier);
        dist = distDefault / averageMultiplier;
    }

    bool CheckDist(Vector3 p, float dist)
    {
        float numerator = a * p.x + b * p.y + c * p.z + d;
        float denominator = Mathf.Sqrt(a * a + b * b + c * c);
        if (numerator < 0)
        {
            numerator = Mathf.Abs(numerator);
            float r = (numerator / denominator);
            if (Mathf.Abs(r - dist) <= dist * 0.01f) return true;
            else return false;
        }
        else return false;
    }

    private void CalculatePlaneABCD(List<Vector3> pointsPlane)
    {
        Vector3 t1, t2, t3;
        float a1, b1, c1, d1;
        t1 = pointsPlane[0];
        t2 = pointsPlane[1];
        t3 = pointsPlane[2];
        a1 = t1.y * (t2.z - t3.z) + t2.y * (t3.z - t1.z) + t3.y * (t1.z - t2.z);
        b1 = t1.z * (t2.x - t3.x) + t2.z * (t3.x - t1.x) + t3.z * (t1.x - t2.x);
        c1 = t1.x * (t2.y - t3.y) + t2.x * (t3.y - t1.y) + t3.x * (t1.y - t2.y);
        d1 = -1 * (t1.x * (t2.y * t3.z - t3.y * t2.z) + t2.x * (t3.y * t1.z - t1.y * t3.z) + t3.x * (t1.y * t2.z - t2.y * t1.z));
        //Debug.Log(a1 + " " + b1 + " " + c1 + " " + d1);

        float a2, b2, c2, d2;
        t1 = pointsPlane[1];
        t2 = pointsPlane[2];
        t3 = pointsPlane[3];
        a2 = t1.y * (t2.z - t3.z) + t2.y * (t3.z - t1.z) + t3.y * (t1.z - t2.z);
        b2 = t1.z * (t2.x - t3.x) + t2.z * (t3.x - t1.x) + t3.z * (t1.x - t2.x);
        c2 = t1.x * (t2.y - t3.y) + t2.x * (t3.y - t1.y) + t3.x * (t1.y - t2.y);
        d2 = -1 * (t1.x * (t2.y * t3.z - t3.y * t2.z) + t2.x * (t3.y * t1.z - t1.y * t3.z) + t3.x * (t1.y * t2.z - t2.y * t1.z));
        //Debug.Log(a2 + " " + b2 + " " + c2 + " " + d2);

        float a3, b3, c3, d3;
        t1 = pointsPlane[0];
        t2 = pointsPlane[2];
        t3 = pointsPlane[3];
        a3 = t1.y * (t2.z - t3.z) + t2.y * (t3.z - t1.z) + t3.y * (t1.z - t2.z);
        b3 = t1.z * (t2.x - t3.x) + t2.z * (t3.x - t1.x) + t3.z * (t1.x - t2.x);
        c3 = t1.x * (t2.y - t3.y) + t2.x * (t3.y - t1.y) + t3.x * (t1.y - t2.y);
        d3 = -1 * (t1.x * (t2.y * t3.z - t3.y * t2.z) + t2.x * (t3.y * t1.z - t1.y * t3.z) + t3.x * (t1.y * t2.z - t2.y * t1.z));
        //Debug.Log(a3 + " " + b3 + " " + c3 + " " + d3);

        float a4, b4, c4, d4;
        t1 = pointsPlane[0];
        t2 = pointsPlane[1];
        t3 = pointsPlane[3];
        a4 = t1.y * (t2.z - t3.z) + t2.y * (t3.z - t1.z) + t3.y * (t1.z - t2.z);
        b4 = t1.z * (t2.x - t3.x) + t2.z * (t3.x - t1.x) + t3.z * (t1.x - t2.x);
        c4 = t1.x * (t2.y - t3.y) + t2.x * (t3.y - t1.y) + t3.x * (t1.y - t2.y);
        d4 = -1 * (t1.x * (t2.y * t3.z - t3.y * t2.z) + t2.x * (t3.y * t1.z - t1.y * t3.z) + t3.x * (t1.y * t2.z - t2.y * t1.z));
        //Debug.Log(a4 + " " + b4 + " " + c4 + " " + d4);

        a = (a1 + a2 + a3 + a4) / 4;
        b = (b1 + b2 + b3 + b4) / 4;
        c = (c1 + c2 + c3 + c4) / 4;
        d = (d1 + d2 + d3 + d4) / 4;
    }
}
