using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemInit : MonoBehaviour
{

    private void OnEnable()
    {
        GetComponent<EventSystem>().SetSelectedGameObject(GetComponent<EventSystem>().firstSelectedGameObject);
    }
}
