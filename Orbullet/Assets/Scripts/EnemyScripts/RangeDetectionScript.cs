using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDetectionScript : MonoBehaviour
{
    private bool inRange = false;

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }*/

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            Debug.Log("Player out of range");
        }
    }

    public bool isInRange()
    {
        return inRange;
    }

}
