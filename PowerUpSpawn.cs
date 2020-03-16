using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawn : MonoBehaviour
{
    [Header("Average Seconds Between Power Up Spawn - ")]
    public float spawnRate = 1.0f;
    [Header("Range Around Average Spawn Time - ")]
    public float spawnTimeRange;

    [Header("Spawn Position & Rotation")]
    public float spawnPosition;
    public float spawnPositionRandomOffset;
    public float spawnRotation = 15f;

    [Header("Toggle PowerUp Spawning")]
    public bool isSpawning = false;


    private float nextPowerUp;
    private float timeSinceLastPowerUp = 0;

    public GameObject[] Spawnables;
    
    // Start is called before the first frame update
    void Start()
    {
        nextPowerUp = spawnRate + Random.Range(-spawnTimeRange, spawnTimeRange);
    }



    // Update is called once per frame
    void Update()
    {
        timeSinceLastPowerUp += TimeSpeed.TIMESPEED.timeChange;
        //Debug.Log(timeSinceLastPowerUp);

        if (timeSinceLastPowerUp >= nextPowerUp && isSpawning) {

            GameObject newPowerUp = Instantiate <GameObject>(Spawnables[Random.Range(0, Spawnables.Length )]);
            float xPosition;
            float yRotation;

            if (Random.Range(0.0f, 10.0f) > 5) {
                xPosition = spawnPosition + Random.Range(spawnPositionRandomOffset, -spawnPositionRandomOffset);
                yRotation = -spawnRotation;
            }
            else {
                xPosition = -spawnPosition + Random.Range(spawnPositionRandomOffset, -spawnPositionRandomOffset);
                yRotation = spawnRotation;
            }

            //Spawn and rotate power up or spawnable item to proper transforms
            newPowerUp.transform.position = new Vector3(xPosition, -0.2f, 100);
            newPowerUp.transform.rotation = Quaternion.Euler(new Vector3(0, yRotation, 0));

            timeSinceLastPowerUp = 0.0f;
            nextPowerUp = spawnRate + Random.Range(-spawnTimeRange, spawnTimeRange);
        }
    }
}
