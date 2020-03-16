using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSpeed : MonoBehaviour
{
        
    static public TimeSpeed TIMESPEED; //create singleton

    [Header("!!!   SINGLETON   !!!")]
    public float timeDilation = 1.0f;
    public float timeChange;
    public float globalMoveSpeed = 10.0f;

    public float speedWithTime;

    //assign singleton to object
    public void Awake() {
        if (TIMESPEED == null) {
            TIMESPEED = this;
        }
        else {
            Debug.Log("Attempted to assign second singleton: TIMESPEED");
            //shouldn't happen
        }
    }

    public void Update() {
        timeChange = Time.deltaTime * timeDilation;

        speedWithTime = timeChange * globalMoveSpeed;
    }
}
