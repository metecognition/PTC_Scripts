using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonMountRotation : MonoBehaviour
{
    public Transform CannonMain;

    private void Update()
    {
        Vector3 cannonRot = CannonMain.rotation.eulerAngles;
        Vector3 newRot = transform.rotation.eulerAngles;
        newRot.y = cannonRot.y;
        transform.eulerAngles = newRot;
    }
}
