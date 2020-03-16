using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    private float moveSpeed;
    //private TimeSpeed timeSpeed;

    public bool destroyAfterRange = false;
    public float destroyDistance = -50f;


    void Update()
    {
        Vector3 movingTo = transform.position;
        movingTo.z -= TimeSpeed.TIMESPEED.speedWithTime;
        transform.position = movingTo;

        //Debug.Log("Working");
        if (destroyAfterRange) {
            if (transform.position.z < destroyDistance) {
                Destroy(gameObject);
            }
        }
        
    }
}
