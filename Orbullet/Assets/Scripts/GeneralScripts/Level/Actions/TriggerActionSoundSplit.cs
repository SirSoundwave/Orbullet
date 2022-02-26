using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActionSoundSplit : TriggerAction
{


    private void Awake()
    {
        action = OnTriggered.SOUNDCLIPINDEX;
    }

    [SerializeField]
    //must be the same length
    private int[] indexes, nextIndexes;

    

    public override void Triggered()
    {
        Debug.Log("It has in fact triggered, you absolute potato");
        foreach(int index in indexes)
        {
            if(handler.getPlayingIndex().Equals(index))
            {
                Debug.Log("I'm in");
                handler.playClip(nextIndexes[Array.IndexOf(indexes, index)], true);
            }
        }
    }
}
