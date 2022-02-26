using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIInit : MonoBehaviour
{
    [SerializeField]
    private bool activeOnAwake = false;
    
    public Button[] buttons;

    public EventSystem system;

    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        //system.gameObject.SetActive(activeOnAwake);
        if (buttons.Length > 0)
        {
            foreach (Button b in buttons)
            {
                b.gameObject.SetActive(activeOnAwake);
            }
            if (activeOnAwake)
            {
                system.SetSelectedGameObject(buttons[0].gameObject);
            }
        }
        canvas.enabled = activeOnAwake;
    }

    public void setUIActive(bool v)
    {


        if (buttons.Length > 0)
        {
            foreach (Button b in buttons)
            {
                b.gameObject.SetActive(v);
            }
            if (v)
            {
                buttons[0].Select();
            }

        }
        canvas.enabled = v;
        Debug.Log("UI active set to " + v);
    }
}
