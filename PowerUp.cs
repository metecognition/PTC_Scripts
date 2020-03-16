using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    private GameObject playerCannon;

    [Header("0 - None, 1 - Explosive, 2 - Rapid")]
    public int powerUpType;
    public float duration = 5.0f;

    private void Start() {
        playerCannon = GameObject.Find("VR/PlayerCannonFinal");
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player Shot") {
            if (powerUpType != 0) {
                playerCannon.GetComponent<CannonAim>().currentPowerUp = powerUpType;
                playerCannon.GetComponent<CannonAim>().powerUpTime = duration;
            }
            Destroy(this.gameObject);
        }
    }
}
