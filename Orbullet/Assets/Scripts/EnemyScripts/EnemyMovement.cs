using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3.5f;

    //collision raycaster stats
    [SerializeField]
    private float raycastGeneralOffset = 2.5f;
    [SerializeField]
    private float raycastVerticalOffset = 2.5f;
    [SerializeField]
    private float detectionDistance = 2f;


    [SerializeField]
    private float turnTime = 5f;
    [SerializeField]
    public Transform target;
    [SerializeField]
    private EnemyAnimator anim;

    private Rigidbody rb;

    private bool playerInRange;
    [SerializeField]
    private GameObject rangeObject;
    private RangeDetectionScript rangeTrigger;
    private GravityControl gravControl;

    public LayerMask obstacleMask;

    private HealthScript health;

    private bool died = false;

    private static bool enemiesHaveControl = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rangeTrigger = rangeObject.GetComponent<RangeDetectionScript>();
        gravControl = GetComponent<GravityControl>();
        anim = GetComponent<EnemyAnimator>();
        health = GetComponentInChildren<HealthScript>();
        enemiesHaveControl = true;
    }

    void FixedUpdate()
    {
        anim.Walk(false);
        if (enemiesHaveControl)
        {
            //kills the drone
            if (health.isDead() && !died)
            {
                Debug.Log("Drone died");
                GetComponent<EnemyAttack>().enabled = false;
                anim.GetAnimator().enabled = false;
                gravControl.setGravActive(true);
                rb.constraints = RigidbodyConstraints.None;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                anim.disableParticles();
                Invoke("DeactivateGameObject", 10f);
                died = true;

            }
            else if (!died)
            {
                if (GetComponent<EnemyAttack>().isTargetVisible())
                {
                    PathFind();
                }
                
            }
        }
    }

    void Move()
    {
        anim.Walk(true);
        transform.position += transform.forward * moveSpeed * Time.fixedDeltaTime; 
    }

    void Turn()
    {
        Vector3 pos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(pos * 20, gravControl.gravityUpDir());
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnTime * Time.fixedDeltaTime);
    }

    //prevents the enemy from colliding with or clipping through walls. Has some trouble with cuboid planets.
    void PathFind()
    {
        RaycastHit hit;
        Vector3 raycastOffset = Vector3.zero;

        //creates detection vectors
        Vector3 vertOffset = new Vector3(0, raycastVerticalOffset, 0f);
        Vector3 left = transform.position + vertOffset - transform.right * raycastGeneralOffset;
        Vector3 right = transform.position + vertOffset + transform.right * raycastGeneralOffset;
        Vector3 up = transform.position + vertOffset + transform.up * raycastGeneralOffset;
        Vector3 down = transform.position + vertOffset - transform.up * raycastGeneralOffset;

        Debug.DrawRay(left, transform.forward * detectionDistance, Color.red);
        Debug.DrawRay(right, transform.forward * detectionDistance, Color.red);
        Debug.DrawRay(up, transform.forward * detectionDistance, Color.red);
        Debug.DrawRay(down, transform.forward * detectionDistance, Color.red);

        //checks the left
        if(Physics.Raycast(left, transform.forward, out hit, detectionDistance, obstacleMask) && !(hit.transform.CompareTag("Player")))
        {
            raycastOffset += transform.TransformDirection(Vector3.up);
        }
        //checks the right
        else if (Physics.Raycast(right, transform.forward, out hit, detectionDistance, obstacleMask) && !(hit.transform.CompareTag("Player")))
        {
            raycastOffset += transform.TransformDirection(Vector3.down);
        }
        //checks the top
        if (Physics.Raycast(up, transform.forward, out hit, detectionDistance, obstacleMask) && !(hit.transform.CompareTag("Player")))
        {
            raycastOffset += transform.TransformDirection(Vector3.right);
        }
        //checks the bottom
        else if (Physics.Raycast(down, transform.forward, out hit, detectionDistance, obstacleMask) && !(hit.transform.CompareTag("Player")))
        {
            raycastOffset += transform.TransformDirection(Vector3.left);
        }

        if (raycastOffset != Vector3.zero)
        {
            transform.Rotate(raycastOffset * 20 * Time.fixedDeltaTime, Space.World);
        } else 
        {
            Turn();
            Move();
        }
    }

    void DeactivateGameObject()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }

    public static void setEnemyControl(bool control)
    {
        enemiesHaveControl = control;
    }

    public static bool getEnemyControl()
    {
        return enemiesHaveControl;
    }

}
