using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator anim;
    [SerializeField]
    private ParticleSystem[] thrusters;
    [SerializeField]
    private ParticleSystem sparkBurst;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    //walk animation
    public void Walk(bool walk)
    {
        anim.SetBool("Walk", walk);
    }
    //run animation
    public void Run(bool run)
    {
        anim.SetBool("Run", run);
    }
    //death animation
    public void Dead()
    {
        anim.SetTrigger("Dead");
    }
    //disables thruster particles
    public void disableParticles()
    {
        foreach (ParticleSystem p in thrusters)
        {
            p.Stop();
        }
    }
    //enables thruster particles
    public void enableParticles()
    {
        foreach (ParticleSystem p in thrusters)
        {
            p.Play();
        }
    }
    //damage animation
    public void damage()
    {
        anim.SetTrigger("Damage");
        sparkBurst.Play();
    }

    public Animator GetAnimator()
    {
        return anim;
    }



}
