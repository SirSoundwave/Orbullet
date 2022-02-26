using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public enum BULLETTYPE
{
    PLAYER,
    ENEMY
}

public class BulletScript : MonoBehaviour
{
    private Rigidbody me;

    [SerializeField]
    private float speed = 30f;

    public float deactivateTimer = 30f;

    public int damage = 15;

    [SerializeField]
    private BULLETTYPE type;


    private void Awake()
    {
        me = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DeactivateGameObject", deactivateTimer);
    }

    public void fire(Camera mainCamera)
    {
            me.velocity = mainCamera.transform.forward * speed;

            transform.LookAt(transform.position + me.velocity);
        
    }

    public void fire(Transform start)
    {
        me.velocity = start.forward * speed;

        transform.LookAt(transform.position + me.velocity);

    }

    void DeactivateGameObject()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other && me.velocity.magnitude > 0.5)
        {
            if (other.CompareTag("Enemy") && type.Equals(BULLETTYPE.PLAYER))
            {
                other.gameObject.GetComponentInParent<HealthScript>().damage(damage);
                //Debug.Log("Bullet hit enemy");
                DeactivateGameObject();
            } else if (other.CompareTag("Player") && type.Equals(BULLETTYPE.ENEMY))
            {
                other.gameObject.GetComponent<HealthScript>().damage(damage);
                //Debug.Log("Bullet hit Player");
                DeactivateGameObject();
            }
            else if (other.CompareTag("Target"))
            {
                other.GetComponent<TargetHitScript>().Activate();
                //Debug.Log("Bullet hit trigger");
                DeactivateGameObject();
            }
        }
        //DeactivateGameObject
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && type.Equals(BULLETTYPE.PLAYER))
        {
            Debug.Log("Bullet hit enemy");
            DeactivateGameObject();
        }
        else if (collision.gameObject.CompareTag("Player") && type.Equals(BULLETTYPE.ENEMY))
        {
            Debug.Log("Bullet hit player");
            DeactivateGameObject();
        }
    }


    // Update is called once per frame
    void Update()
    {
    }
}
