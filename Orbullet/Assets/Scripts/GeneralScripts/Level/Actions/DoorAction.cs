using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis
{
    X,
    Y,
    Z
}

public class DoorAction : ObjectAction
{
    [SerializeField]
    Axis axis = Axis.X;
    [SerializeField]
    float distance = 5f;
    [SerializeField]
    float speed = 2f;
    [SerializeField]
    bool invertMoveDir = false;
    int dir = 1;
    Vector3 targetPos;
    Vector3 moveDir;
    Vector3 originalPos;

    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        
        if (invertMoveDir)
        {
            dir = -1;
        }
        //rb = GetComponent<Rigidbody>();
        switch(axis){
            case Axis.X:
            {
                moveDir = new Vector3(1, 0, 0) * dir;
                break;
            }
            case Axis.Y:
            {
                moveDir = new Vector3(0, 1, 0) * dir;
                break;
            }
            case Axis.Z:
            {
                moveDir = new Vector3(0, 0, 1) * dir;
                break;
            }
            default: break;

        }
        targetPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active && Vector3.Distance(originalPos + moveDir * distance, transform.position) != 0)
        {
            //Debug.Log("Active");
            moveDoor();
        }
    }

    public void moveDoor()
    {
        //Debug.Log("Moving door");
        if(Vector3.Distance(originalPos + moveDir * distance, transform.position) >= speed * Time.fixedDeltaTime)
        {
            transform.position += moveDir * speed * Time.fixedDeltaTime;
        } else
        {
            transform.position = originalPos + moveDir * distance;
        }
        
    }

}
