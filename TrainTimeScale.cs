using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTimeScale : MonoBehaviour
{

    private TimeSpeed timeSpeed;

    private ParticleSystem ps;

    void Start() {
        timeSpeed = GameObject.Find("GameManager").GetComponent<TimeSpeed>();
        ps = this.GetComponent<ParticleSystem>();
    }


    void Update()
    {
        var main = ps.main;
        main.simulationSpeed = timeSpeed.timeDilation;
    }
}
