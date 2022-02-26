using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!(other.CompareTag("Ignore")))
        {
            HealthScript h = other.GetComponentInParent<HealthScript>();
            h.damage(h.health);
        }
    }
}
