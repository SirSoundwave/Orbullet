using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OnTriggered
{
    LIGHT,
    DESTROY,
    TOGGLEACTIVITY,
    SOUNDCLIP,
    SOUNDCLIPINDEX,
    NONE
}


public class TriggerAction : MonoBehaviour
{
    [SerializeField]
    protected OnTriggered action;
    //Only required for SOUNDCLIPINDEX
    [SerializeField]
    private int clipIndex = 0;
    [SerializeField]
    protected AudioHandler handler;

    public virtual void Triggered()
    {
        Debug.Log("Trigger activated");
        if (action.Equals(OnTriggered.LIGHT))
        {
            MeshRenderer render = GetComponent<MeshRenderer>();
            render.material.EnableKeyword("_EMISSION");
            render.material.SetColor("_EmissionColor", render.material.color);
        } else if (action.Equals(OnTriggered.DESTROY))
        {
            //add explosion or something
            GameObject me = GetComponent<GameObject>();
            if (me.activeInHierarchy)
            {
                me.SetActive(false);
            }
        } else if (action.Equals(OnTriggered.TOGGLEACTIVITY))
        {
            GetComponent<ObjectAction>().Activate();
        } else if (action.Equals(OnTriggered.SOUNDCLIP))
        {
            handler.playNextClip();
        } else if (action.Equals(OnTriggered.SOUNDCLIPINDEX))
        {
            handler.playClip(clipIndex, true);
        }
    }

}
