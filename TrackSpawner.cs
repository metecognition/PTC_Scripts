using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSpawner : MonoBehaviour
{
    public GameObject trackPiece;
    public float trackLength = 10.0f;
    public float trackSpeed;
    private float distanceSinceLastTrack = 0;
    public int trackDistance = 5;
    public Vector3 trackStart;


    void Start() {
        trackStart = new Vector3(0, 0, trackDistance);
        for(int i = 0; i <= trackDistance; i++) {
            GameObject newTrack = Instantiate<GameObject>(trackPiece);
            Vector3 trackPosition = new Vector3(transform.position.x, transform.position.y, i * 10);
            newTrack.transform.position = trackPosition;

            if (Random.Range(0, 2) >= 1) {
                newTrack.transform.localScale = new Vector3(-1, 1, 1);
                //Debug.Log("Flip Track");
            }

        }

    }


    // Update is called once per frame
    void Update()
    {
        trackSpeed = TimeSpeed.TIMESPEED.speedWithTime;

        distanceSinceLastTrack += trackSpeed;

        if (distanceSinceLastTrack >= 10) {
            GameObject newTrack = Instantiate<GameObject>(trackPiece);
            Vector3 trackPosition = new Vector3(transform.position.x, transform.position.y, (10+(10*trackDistance)) - distanceSinceLastTrack);
            newTrack.transform.position = trackPosition;

            if (Random.Range(0, 2) >= 1) {
                newTrack.transform.localScale = new Vector3(-1, 1, 1);
                //Debug.Log("Flip Track");
            }

            distanceSinceLastTrack -= 10.0f;
        }

    }
}
