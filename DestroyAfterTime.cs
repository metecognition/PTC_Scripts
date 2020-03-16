using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifetime;

    private void Update() {
        lifetime -= TimeSpeed.TIMESPEED.timeChange;
        if (lifetime <= 0) {
            Destroy(this.gameObject);
        }
    }
}
