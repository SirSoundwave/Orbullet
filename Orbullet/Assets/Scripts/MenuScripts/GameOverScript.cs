using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameOverScript : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private HealthScript health;
    private FirstPersonMovement movement;
    private Canvas canvas;

    private Button[] buttons;

    [SerializeField]
    private EventSystem system;

    private bool activated = false;

    private UIInit init;
 
    void Awake()
    {
        health = player.GetComponent<HealthScript>();
        movement = player.GetComponent<FirstPersonMovement>();
        canvas = GetComponent<Canvas>();
        init = GetComponent<UIInit>();
        buttons = init.buttons;
        system = init.system;
    }

    void Update()
    {
        if (health.isDead() && !activated)
        {
            activated = true;
            //enables Game Over screen on player death
            movement.setPlayerControl(false);
            EnemyMovement.setEnemyControl(false);
            init.setUIActive(true);
            Debug.Log("Gameover");
        }
    }

    public void restartLevel()
    {
        activated = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        init.setUIActive(false);
    }

}
