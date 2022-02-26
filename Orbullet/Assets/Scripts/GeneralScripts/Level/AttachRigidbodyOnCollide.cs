using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachRigidbodyOnCollide : MonoBehaviour
{
    //which tag should be attached
    [SerializeField]
    private string tag;
    //which rigidbodies are attached
    private List<Rigidbody> bodies = new List<Rigidbody>();

    private Vector3 lastPos;
    private Transform trans;

    private void Start()
    {
        trans = transform;
        lastPos = trans.position;
        
    }

    private void LateUpdate()
    {
        if(bodies.Count > 0)
        {
            foreach (Rigidbody rb in bodies)
            {
                //transforms the attached rigidbodies with the movement of the platforms
                Vector3 vel = (trans.position - lastPos);
                rb.transform.Translate(vel, trans);
            }
        }
        lastPos = trans.position;
    }

    //adds bodies of the determined tag to the list of attached bodies on collision with the platform
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tag))
        {
            AddBody(other.GetComponent<Rigidbody>());
        }
    }
    //removes bodies of the determined tag to the list of attached bodies on collision with the platform
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tag))
        {
            RemoveBody(other.GetComponent<Rigidbody>());
        }
    }

    private void AddBody(Rigidbody body)
    {
        if (!bodies.Contains(body))
        {
            bodies.Add(body);
        }
        
    }

    private void RemoveBody(Rigidbody body)
    {
        if (bodies.Contains(body))
        {
            bodies.Remove(body);
        }
    }

}
