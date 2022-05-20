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
            gravityUp = getGravNormals();
            
            if (applyGravity)
            {
                //Rotate
                Vector3 localUp = transform.up.normalized;

                Quaternion targRot = Quaternion.FromToRotation(localUp, gravityUp) * transform.rotation;

                transform.rotation = (Quaternion.SlerpUnclamped(transform.rotation, targRot, rotationSpeed * Time.deltaTime));
                Debug.Log(transform.rotation);

                //Move
                rb.AddForce((-gravityUp * Gravity.Gravity) * rb.mass);
                Debug.DrawRay(transform.position, (-gravityUp * Gravity.Gravity) * rb.mass);
            }
            


        }
    }
    void Update()
    {
        if (applyGravity)
        {
            
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


    public Vector3 getGravNormals()
    {
        //Checks for linear gravity
        if (Gravity.fixedDirection)
        {
            //Sets the up gravity direction to the up direction of the object this script is applied to
            return Gravity.transform.up.normalized;

        }
        //Checks for mesh based gravity
        else if (Gravity.complexGravity)
        {
            //Locates the closest mesh triangle
            int triIndex = getClosestTriangleIndex();
            Vector3 ret;
            if (triIndex >= 0)
            {
                lastTriIndex = triIndex;
                ret = (getTriangleNormal(triIndex));
                highlightTriangle(triIndex);
                drawTriangleNormal(triIndex);
            }
            else
            {
                ret = (getTriangleNormal(lastTriIndex));
                highlightTriangle(lastTriIndex);
                drawTriangleNormal(lastTriIndex);
            }
            return ret.normalized;
        }
        //Returns the gravity normal as a vector pointing from a center point towards the object this script is attached to
        else
        {
            return (transform.position - Gravity.transform.position).normalized;

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
