using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum restoreType
{
    HEALTH,
    AMMO
}

public class RestoreStatObject : MonoBehaviour
{
    [SerializeField]
    private restoreType type;

    [SerializeField]
    private int amount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (type.Equals(restoreType.AMMO))
            {
                other.GetComponent<PlayerStats>().fillAmmo(amount);
            }
            else if (type.Equals(restoreType.HEALTH))
            {
                other.GetComponent<PlayerStats>().fillHealth(amount);
            }
            gameObject.SetActive(false);
        }
    }
}
