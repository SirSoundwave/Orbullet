using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    private static AudioSource source;

    //all serialized arrays MUST be the same length
    [SerializeField]
    private AudioClip[] clips;
    [SerializeField]
    private int[] nextIndexes;
    [SerializeField]
    private bool[] continueAudio;
    private int currentClip;
    private int nextClip;

    [SerializeField]
    private bool playOnAwake = false;

    private int[] queuedClipIndex = { -1, -1};


    private void Awake()
    {
        source = GetComponent<AudioSource>();
        nextClip = 0;
        Debug.Log("Queued clip 0: " + queuedClipIndex[0] + "; Queued clip 1: " + queuedClipIndex[1]);
        if (playOnAwake)
        {
            source.Stop();
            playNextClip();
        }
        
    }

    private void Update()
    {
        if(!isClipPlaying())
        {
            //plays the queued clip if the index exists
            if(!(queuedClipIndex[0] < 0)){
                //Debug.Log("playing queued clip; index: " + queuedClipIndex[0]);
                playClip(queuedClipIndex[0], queuedClipIndex[1], false, false, true);
            }
            //plays the next clip
            else if (currentClip < clips.Length && continueAudio[currentClip])
            {
                //Debug.Log("playing next clip");
                playNextClip();
            }
        }
        
    }

    public void playNextClip()
    {
        playClip(nextClip, false);
        
    }
    public void playClip(int index, bool stopCurrent)
    {
        playClip(index, index, stopCurrent, true, false);
    }

    public void playClip(int index, int nextIndex, bool stopCurrent, bool useIndexArray, bool updateQue)
    {
        if (index < clips.Length && index >= 0)
        {
            //stops the current clip (not used on playNextClip())
            if (stopCurrent)
            {
                source.Stop();
            }
            //if a clip is playing, assigns values to the que
            if(isClipPlaying() && !stopCurrent)
            {
                //Debug.Log("Queuing clips");
                //Prevents index loops by clearing the que if both indexes are the same (if playClip() was used)
                if(index == nextIndex)
                {
                    queuedClipIndex[0] = -1;
                    queuedClipIndex[1] = -1;
                    return;
                }
                queuedClipIndex[0] = index;
                queuedClipIndex[1] = nextIndex;
                return;
            //updates the que (might not be needed, we'll see
            }if (updateQue)
            {
            queuedClipIndex[0] = nextClip;
            queuedClipIndex[1] = -1;
            }

            source.PlayOneShot(clips[index]);
            currentClip = index;
            //directly sets the next clip based on nextIndex
            if ((!useIndexArray))
            {
                nextClip = nextIndex;
                //Debug.Log("Setting next index using variable: " + nextIndex);
                //Debug.Log("Next clip = " + nextClip);
                if (continueAudio[index])
                {
                    playNextClip();
                }
            //sets nextClip based on the nextIndexes[] array
            } else if ((nextIndex < nextIndexes.Length) && nextIndex >= 0)
            {
                nextClip = nextIndexes[nextIndex];
                //Debug.Log("Setting next index using array");
                if (continueAudio[index])
                {
                    
                   playNextClip();
                }
            }
            //triggered if next index was out of bounds
            else
            {
                Debug.LogError("Audio Clip next index out of bounds");
            }
            
        }
        //triggered if the audio index is out of bounds
        else
        {
            Debug.LogError("Audio Clip index out of bounds");
        }
    }

    public bool isClipPlaying()
    {
        return source.isPlaying;
    }
    public int getPlayingIndex()
    {
        if (source.isPlaying)
        {
            return currentClip;
        }
        return -1;
    }
}
