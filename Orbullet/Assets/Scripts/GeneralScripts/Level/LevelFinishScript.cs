﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinishScript : MonoBehaviour
{

    public GameObject endScreen;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            endScreen.gameObject.SetActive(true);
            endScreen.GetComponent<UIInit>().setUIActive(true);
            other.GetComponent<FirstPersonMovement>().setPlayerControl(false);
            EnemyMovement.setEnemyControl(false);
        }
    }

}
