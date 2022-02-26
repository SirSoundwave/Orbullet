using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float fireRate = 15f;
    private float nextTimeToFire;
    public float damage = 20f;

    //script for a trigger object to check if a player is inside it
    private RangeDetectionScript rangeDetection;


    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform startPosition;

    public GameObject audioSource;

    [SerializeField]
    private AudioClip fireSound;

    [SerializeField]
    public LayerMask targetLayers;

    private AudioSource fireSoundSource;

    private EnemyMovement mov;
    void Awake()
    {
        fireSoundSource = audioSource.GetComponent<AudioSource>();
        rangeDetection = GetComponentInChildren<RangeDetectionScript>();
        nextTimeToFire = Time.time;
        mov = GetComponent<EnemyMovement>();
    }

    void FixedUpdate()
    {
        Debug.DrawRay((transform.position), (mov.target.transform.position - transform.position) * 5, Color.green);
        //shoots if the player is in range and if cooldown is expired
        if (isTargetVisible() && rangeDetection.isInRange() && (Time.time >= nextTimeToFire) && EnemyMovement.getEnemyControl())
        {
            WeaponShoot();
            nextTimeToFire = Time.time + fireRate;
            fireSoundSource.PlayOneShot(fireSound);
        }
        
    }

    void WeaponShoot()
    {
        GameObject bullet = Instantiate(bulletPrefab);

        bullet.transform.position = startPosition.position;
        bullet.GetComponent<GravityControl>().Gravity = GetComponentInParent<GravityControl>().Gravity;
        bullet.GetComponent<BulletScript>().fire(startPosition);
    }
    public bool isTargetVisible()
    {

        Ray detectDir = new Ray(transform.position, Camera.main.transform.forward);
        


        if (!(Physics.Raycast(transform.position, (mov.target.transform.position - transform.position), 5, targetLayers)))
        {
            Debug.Log("Target Visible");
            return true;
            

        }
        Debug.Log("Target Not Visible");
        return false;

    }

}
