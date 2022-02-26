using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonMovement : MonoBehaviour
{
    //mouse sensitivity
    public float mouseSenseX = 250;
    public float mouseSenseY = 250;

    public float jumpForce = 1227;

    Transform camT;

    float vertLookRot = 0;
    Vector3 moveAm;
    Vector3 smoothMoveVel;

    //what counts as ground
    public LayerMask groundedMask;

    //is the player touching ground
    bool grounded = true;
    
    public float walkSpeed = 7;

    //gravity controller
    private GravityControl gravCont;
    Rigidbody rb;

    //should the player have control
    private bool playerHasControl = true;

    [SerializeField]
    private GameObject start;

    PlayerInput input;

    Vector2 RawLeft;
    Vector2 RawRight;

    private bool targeting = false;
    private GameObject lastTarget;
    RaycastHit lastHit;
    public int maxTargetDistance = 20;
    public LayerMask targetLayers;

    private RaycastHit groundHit;
    private void Awake()
    {
        //sets the main camera to the player camera
        camT = Camera.main.transform;
        rb = transform.GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        //gets the gravity controller
        gravCont = transform.GetComponent<GravityControl>();
        //teleports player to start position
        transform.position = start.transform.position;

        input = new PlayerInput();

        //Jump button; ctx = action context; In this case, ctx is ignored
        input.Gameplay.Jump.performed += ctx => Jump();
        //Player Movement
        input.Gameplay.Movement.performed += ctx => RawLeft = ctx.ReadValue<Vector2>();
        input.Gameplay.Movement.canceled += ctx => RawLeft = Vector2.zero;

        //Camera movement
        input.Gameplay.Camera.performed += ctx => RawRight = ctx.ReadValue<Vector2>();
        input.Gameplay.Camera.canceled += ctx => RawRight = Vector2.zero;
        input.Gameplay.CenterCamera.performed += ctx => CenterCamera();

        //Target enemies
        input.Gameplay.Target.performed += ctx => targeting = true;
        input.Gameplay.Target.canceled += ctx => targeting = false;

    }

    void Jump()
    {
        if (grounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    void CenterCamera()
    {
        vertLookRot = 0;
    }

    private void OnEnable()
    {
        input.Gameplay.Enable();
    }

    private void OnDisable()
    {
        input.Gameplay.Disable();
    }

    private bool closestTargetInRange(out GameObject target, out RaycastHit hit)
    {
        RaycastHit info;
        
        Ray detectDir = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        Debug.DrawRay((Camera.main.transform.position), Camera.main.transform.forward * 30, Color.green);
        if (Physics.Raycast(detectDir, out info, maxTargetDistance, targetLayers, QueryTriggerInteraction.Collide))
        {
            //Debug.Log("Spherecast collided");
            if (info.collider.tag.Equals("Enemy") || info.collider.tag.Equals("Target"))
            {
                Debug.Log("targeted: " + info.transform.gameObject.name + " at: " + info.distance);
                target = info.collider.gameObject;
                hit = info;
                return true;
            }
            Debug.Log(info.collider.tag);
        }
        target = null;
        hit = new RaycastHit();
        return false;
    }

    private void Update()
    {
        if (playerHasControl)
        {
            if (targeting)
            {
                
                if (false && lastTarget != null && lastTarget.activeInHierarchy && (transform.position - lastTarget.transform.position).magnitude < maxTargetDistance)
                {
                    transform.LookAt(lastTarget.transform.position, gravCont.gravityUpDir());
                    Camera.main.transform.LookAt(lastTarget.transform.position, gravCont.gravityUpDir());
                }
                else if (closestTargetInRange(out lastTarget, out lastHit))
                {
                    Vector3 localPosChar = transform.InverseTransformDirection(lastTarget.transform.position - transform.position);
                    localPosChar.y = 0;
                    Vector3 lookPosChar = transform.position + transform.TransformDirection(localPosChar);
                    Vector3 localPosCam = Camera.main.transform.InverseTransformDirection(lastTarget.transform.position - Camera.main.transform.position);
                    localPosCam.x = 0;
                    //localPosCam.z = 0;
                    Vector3 lookPosCam = Camera.main.transform.position + Camera.main.transform.TransformDirection(localPosCam);
                    Camera.main.transform.LookAt(lookPosCam, gravCont.gravityUpDir());
                    transform.LookAt(lookPosChar, gravCont.gravityUpDir());
                }
            } else
            {
                //rotates player camera horizontally (on local x axis)
                transform.Rotate(Vector3.up * RawRight.x * Time.deltaTime * mouseSenseX);
                //rotates player camera vertically (on local y axis)
                vertLookRot += RawRight.y * Time.deltaTime * mouseSenseY;
                vertLookRot = Mathf.Clamp(vertLookRot, -60, 60);
                camT.localEulerAngles = Vector3.left * vertLookRot;
            }



            //gets movement direction
            Vector3 moveDir = new Vector3(RawLeft.x, 0, RawLeft.y).normalized;

            Vector3 targMoveAm = moveDir * walkSpeed;
            //creates movement vector
            moveAm = Vector3.SmoothDamp(moveAm, targMoveAm, ref smoothMoveVel, .15f);

            grounded = false;
            //downward raycast to determine if player is grounded
            Ray ray = new Ray(transform.position, -transform.up);
            if (Physics.Raycast(ray, out groundHit, 1.1f, groundedMask))
            {
                grounded = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (playerHasControl)
        {
            //draws a ray in front of the player to prevent clipping through walls
            Ray ray = new Ray(transform.position, transform.TransformDirection(moveAm));
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.TransformDirection(moveAm), Color.blue);
            if (!Physics.Raycast(ray, out hit, 1, groundedMask))
            {
                //moves the player's rigid body locally on the movement vector
                rb.transform.Translate(moveAm * Time.fixedDeltaTime, rb.transform);
            }
        }
    }

    public void setPlayerControl(bool control)
    {
        playerHasControl = control;
    }

    public bool getPlayerControl()
    {
        return playerHasControl;
    }



}
