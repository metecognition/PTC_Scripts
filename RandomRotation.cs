using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    void Start()
    {
        Vector3 newRot = new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f);
        transform.rotation = Quaternion.Euler(newRot);
    }
}
