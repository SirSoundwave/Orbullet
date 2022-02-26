using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    //private WeaponManager wepMan;
    public float fireRate = 15f;
    public float nextTimeToFire;
    public float damage = 20f;

    

    

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform startPosition;

    public GameObject audioSource;

    [SerializeField]
    private AudioClip fireSound;

    private AudioSource fireSoundSource;

    private PlayerStats stats;

    private Vector3 recoilDir;
    [SerializeField]
    private float recoilAmt = 1;

    private Rigidbody rb;
    private FirstPersonMovement playerMovement;

    PlayerInput input;

    void Awake()
    {
        //wepMan = GetComponent<WeaponManager>();
        stats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<FirstPersonMovement>();

        input = new PlayerInput();

        input.Gameplay.Shoot.performed += ctx => WeaponShoot();

    }
    private void OnEnable()
    {
        input.Gameplay.Enable();
    }

    private void OnDisable()
    {
        input.Gameplay.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        fireSoundSource = audioSource.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void WeaponShoot()
    {
        if((stats.getAmmo().fillAmount > 0f) && playerMovement.getPlayerControl())
        {
            BulletFired();
            fireSoundSource.PlayOneShot(fireSound, 1f);
            stats.displayAmmoStats(1);
            //recoil for the player
            recoilDir = -Camera.main.transform.forward;
            rb.AddForce(recoilDir * recoilAmt);
        }
            //AudioSource.PlayClipAtPoint(fireSound.clip, transform.position);
    }

    void BulletFired(){
        //creates new bullet
        GameObject bullet = Instantiate(bulletPrefab);
        //fires bullet
        bullet.transform.position = startPosition.position;
        bullet.GetComponent<GravityControl>().Gravity = GetComponentInParent<GravityControl>().Gravity;
        bullet.GetComponent<BulletScript>().fire(Camera.main);
        
    }
}
