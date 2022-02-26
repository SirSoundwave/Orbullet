using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControl : MonoBehaviour
{
    public GravityOrbit Gravity;
    private Rigidbody rb;

    public float rotationSpeed = 20f;

    [SerializeField]
    private bool applyGravity;

    private Vector3 gravityUp = Vector3.zero;

    private Vector3[] normals;

    private Mesh gravMesh;
    private Collider gravCol;

    [SerializeField]
    private LayerMask layerMask;

    private int lastTriIndex = 0;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Gravity)
        {
            if (Gravity.fixedDirection)
            {
                gravityUp = Gravity.transform.up;
            } else if (Gravity.complexGravity)
            {
                int triIndex = getClosestTriangleIndex();
                if(triIndex >= 0)
                {
                    lastTriIndex = triIndex;
                    gravityUp = (getTriangleNormal(triIndex));
                    highlightTriangle(triIndex);
                    drawTriangleNormal(triIndex);
                } else
                {
                    gravityUp = (getTriangleNormal(lastTriIndex));
                    highlightTriangle(lastTriIndex);
                    drawTriangleNormal(lastTriIndex);
                }
                gravityUp.Normalize();
            } else
            {
                gravityUp = (transform.position - Gravity.transform.position).normalized;

            }
            

            Vector3 localUp = transform.up.normalized;

            Quaternion targRot = Quaternion.FromToRotation(localUp, gravityUp) * transform.rotation;
            rb.rotation = Quaternion.Slerp(rb.rotation, targRot, rotationSpeed * Time.fixedDeltaTime);
            if (applyGravity)
            {
                rb.AddForce((-gravityUp * Gravity.Gravity) * rb.mass);
            }
            


        }
    }

    public Vector3 gravityUpDir()
    {
        return gravityUp;
    }

    public void setGravActive(bool b)
    {
        applyGravity = b;
    }

    
    public void getGravNormals()
    {
        normals = gravMesh.normals;
        var triangles = gravMesh.triangles;
        Vector3 faceNormal;
        for (int i = 0; i < triangles.Length / 3; i+=3)
        {

            //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //cube.transform.position = mesh.vertices;
            faceNormal = (normals[triangles[i + 0]] + normals[triangles[i + 1]] + normals[triangles[i + 2]]) / 3.0f;
            faceNormal.Normalize();
        }
    }

    public int getClosestTriangleIndex()
    {
        RaycastHit info;
        Debug.DrawRay(transform.position, (Gravity.triObj.transform.position - transform.position));
        if(Physics.Raycast(transform.position, (Gravity.triObj.transform.position - transform.position), out info) && info.collider != null)
        {
            Debug.Log("Retrieved Triangle Index: " + info.triangleIndex);
            if(info.triangleIndex < 0)
            {
                return lastTriIndex;
            }
            return info.triangleIndex;
        }
        Debug.Log("No Triangle Index");
        return lastTriIndex;

    }

    public Vector3 getTriangleNormal(int triIndex)
    {
        gravMesh = Gravity.triObj.GetComponent<MeshFilter>().mesh;
        Debug.Log("Retrieved Triangle Normal");
        Vector3[] vertices = gravMesh.vertices;
        int[] triangles = gravMesh.triangles;
        normals = gravMesh.normals;
        Vector3 p0 = vertices[triangles[triIndex * 3 + 0]];
        Vector3 p1 = vertices[triangles[triIndex * 3 + 1]];
        Vector3 p2 = vertices[triangles[triIndex * 3 + 2]];

        Vector3 center = ((p0 + p1 + p2) / 3);


        p0 = normals[triangles[triIndex * 3 + 0]];
        p1 = normals[triangles[triIndex * 3 + 1]];
        p2 = normals[triangles[triIndex * 3 + 2]];


        Vector3 faceNormal = ((p0 + p1 + p2) / 3);
        faceNormal.Normalize();
        return faceNormal;
    }
    public void highlightTriangle(int triIndex)
    {
        Vector3[] vertices = gravMesh.vertices;
        int[] triangles = gravMesh.triangles;
        Vector3 p0 = vertices[triangles[triIndex * 3 + 0]];
        Vector3 p1 = vertices[triangles[triIndex * 3 + 1]];
        Vector3 p2 = vertices[triangles[triIndex * 3 + 2]];
        Transform hitTransform = Gravity.triObj.GetComponent<Collider>().transform;
        p0 = hitTransform.TransformPoint(p0);
        p1 = hitTransform.TransformPoint(p1);
        p2 = hitTransform.TransformPoint(p2);
        Debug.DrawLine(p0, p1);
        Debug.DrawLine(p1, p2);
        Debug.DrawLine(p2, p0);
    }
    public void drawTriangleNormal(int triIndex)
    {
        Vector3[] vertices = gravMesh.vertices;
        int[] triangles = gravMesh.triangles;
        Vector3 p0 = vertices[triangles[triIndex * 3 + 0]];
        Vector3 p1 = vertices[triangles[triIndex * 3 + 1]];
        Vector3 p2 = vertices[triangles[triIndex * 3 + 2]];
        Transform hitTransform = Gravity.triObj.GetComponent<Collider>().transform;
        p0 = hitTransform.TransformPoint(p0);
        p1 = hitTransform.TransformPoint(p1);
        p2 = hitTransform.TransformPoint(p2);
        Vector3 pA = (p0 + p1 + p2) / 3;
        Debug.DrawRay(pA,getTriangleNormal(triIndex), Color.blue);
    }
}
