using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectAction : MonoBehaviour
{
    protected bool active = false;
    public void Activate()
    {
        //Debug.Log("Object Action Activated");
        active = true;
    }
    public void Deactivate()
    {
        //Debug.Log("Object Action Toggled");
        active = false;
    }
    public void Toggle()
    {
        //Debug.Log("Object Action Deactivated");
        active = !active;
    }
}
