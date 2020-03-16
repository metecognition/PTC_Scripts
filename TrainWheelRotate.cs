using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainWheelRotate : MonoBehaviour
{
    public float rotateSpeed = 80.0f;
    private float currentRot;

    private void Start()
    {
        currentRot = Random.Range(0.0f, 360.0f);
    }

    private void Update()
    {
        currentRot += rotateSpeed * TimeSpeed.TIMESPEED.timeChange;
        if (currentRot > 360.0f) { currentRot -= 360.0f; }

        transform.rotation = Quaternion.Euler(new Vector3(currentRot, 0.0f, 0.0f));
    }
}
