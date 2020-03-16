using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCannonBallAOE : MonoBehaviour
{
    public GameObject explosionAOE;


    private void OnCollisionEnter(Collision collision) {
        GameObject ex = Instantiate(explosionAOE);
        ex.transform.position = transform.position;

        
    }
}
