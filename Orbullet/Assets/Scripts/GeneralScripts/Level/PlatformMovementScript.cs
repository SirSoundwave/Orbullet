using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovementScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] points;
    private int nextPointIndex;
    private int lastPointIndex;
    private bool invert = false;
    private Vector3 originalPos;
    private bool positionReached = false;

    [SerializeField]
    float speed = 5;


    void Awake()
    {
        //sets up the travel logic variables
        lastPointIndex = 0;
        nextPointIndex = 1;
        transform.position = points[0].transform.position;
        //moves the platform to the first point
        originalPos = transform.position;
    }

    void FixedUpdate()
    {
        moveToNextPoint();
    }

    void moveToNextPoint()
    {
        if (Vector3.Distance(points[nextPointIndex].transform.position, transform.position) >= speed * Time.fixedDeltaTime)
        {
            //moves the platform towards the next point at the determined speed
            transform.position += (points[nextPointIndex].transform.position - transform.position).normalized * speed * Time.fixedDeltaTime;

        }
        else
        {
            transform.position = points[nextPointIndex].transform.position;
            positionReached = true;
        }
        lastPointIndex = nextPointIndex;
        //swaps directions if the first or last point in the array is reached
        if (positionReached)
        {
            if (invert)
            {
                if (lastPointIndex == 0)
                {
                    invert = false;
                    nextPointIndex = 1;
                }
                else
                {
                    nextPointIndex -= 1;
                }
            }
            else
            {
                if (lastPointIndex == points.Length - 1)
                {
                    invert = true;
                    nextPointIndex = lastPointIndex - 1;
                }
                else
                {
                    nextPointIndex += 1;
                }
            }
            positionReached = false;
        }
    }

}
