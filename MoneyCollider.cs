using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCollider : MonoBehaviour
{

    private void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnCollisionEnter(Collision other)
    {

        //If the collider is a enemy cannon shot, then the money count goes down

        if (other.gameObject.CompareTag("Enemy Shot")){
            ScoreMoneyManager.S.LoseMoney(1.0f * TrainAIManager.Master.difficulty);
        }
    }
}
