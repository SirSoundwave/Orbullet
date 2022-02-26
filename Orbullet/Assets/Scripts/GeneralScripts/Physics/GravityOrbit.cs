using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityOrbit : MonoBehaviour
{
    public float Gravity = 9.8F;

    public bool fixedDirection;

    public MeshCollider triObj;

    public bool complexGravity = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GravityControl>())
        {
            other.GetComponent<GravityControl>().Gravity = this.GetComponent<GravityOrbit>();
        }
    }
}
