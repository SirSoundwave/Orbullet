using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHitScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] linkedObjects;

    public void Activate()
    {
        Debug.Log("bullet hit trigger");
        foreach (GameObject obj in linkedObjects){
            if(obj.activeInHierarchy == false)
            {
                obj.SetActive(true);
            }
            
            obj.GetComponent<TriggerAction>().Triggered();
        }
        
    }



}
