using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float health = 150.0f;
    private bool dead = false;

    private EnemyAnimator anim;

    void Awake()
    {
        if (transform.CompareTag("Player"))
        {
            GetComponent<PlayerStats>().maxHealth = health;
        }
        if (transform.CompareTag("Enemy"))
        {
            anim = GetComponentInParent<EnemyAnimator>();
        }
    }
    /*
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Death") && !(this.CompareTag("Ignore")))
        {
            damage(health);
        }
    }
    */
    public void damage(float amt)
    {
        if (dead)
        {
            return;
        }
        health -= amt;
        if (transform.CompareTag("Player"))
        {
            GetComponent<PlayerStats>().displayHealthStats(amt);
        } if (transform.CompareTag("Enemy"))
        {
            anim.damage();
        }
        if (health <= 0f)
        {
            if (GetComponent<Rigidbody>())
            {
                GetComponent<Rigidbody>().AddTorque(-transform.forward * 5f);
            } else
            {
                GetComponentInParent<Rigidbody>().AddTorque(-transform.forward * 5f);
            }
            
            dead = true;
        }
    }

    public bool isDead()
    {
        return dead;
    }
}
