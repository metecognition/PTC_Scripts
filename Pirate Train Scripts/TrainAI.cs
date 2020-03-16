using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainAI : MonoBehaviour
{
    //state options are created here, then added to the switch statement in update, which calls the function for the current state
    private enum State {
        start,
        retreat,
        attackRandom,
        attackFull,
        idle
    }

    [Header("Movement")]
    public float farthestAway = 0.0f;
    public float positionOffset = -5.0f;
    public float maxMoveSpeed = 1.0f;
    public float moveSpeedChange = 0.05f;
    public GameObject playerTrain;
    private float currentMoveSpeed;
    private bool movingForward = true;
    private float currentDistance;
    private float idleDistance;


    [Header("Cannon Setup")]
    public GameObject[] cannons;
    public GameObject pirateCannonBall;
    public float cannonReloadTime = 2.5f;
    public float randomReloadVariation = 0.7f;
    public GameObject cannonFireEffect;
    public AudioClip[] cannonSoundEffects;
    private float currentReloadTime;
    public PirateCannon[] cannonControllers;

    
    [Header("State Setup")]
    public float averageTimeToFireAll = 15.0f;
    public float fireAllVariation = 2.0f;
    public float retreatTime = 10.0f;
    public float difficultyIncrease = 0.01f;
    private float timeToFireAll;
    //private bool shouldRetreat = false;
    //private float retreatTimer = 0.0f;
    private State currentState;
    private float cannonsEnabled;
    //private bool hasDisabledCannonsOnRetreat = false;


    void Start()
    {

        cannonControllers = new PirateCannon[cannons.Length];
        for (int i = 0; i < cannons.Length; i++) {
            cannonControllers[i] = cannons[i].GetComponent<PirateCannon>();
            cannonControllers[i].reloadTime = cannonReloadTime;
            cannonControllers[i].randomReloadVariation = randomReloadVariation;
            cannonControllers[i].cannonBall = pirateCannonBall;
            cannonControllers[i].difficultyIncrease = difficultyIncrease;
            cannonControllers[i].cannonFireEffect = cannonFireEffect;
        }

        timeToFireAll = averageTimeToFireAll + Random.Range(-fireAllVariation, fireAllVariation);
        SetFireAtWill(false);
        currentDistance = transform.position.z - playerTrain.transform.position.z;
        currentState = State.idle;
        idleDistance = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {

        currentDistance = transform.position.z - playerTrain.transform.position.z + positionOffset;

        //switch statement for state machine
        //each state should have at least one exit condition to switch to a different state

        switch (currentState) {
            case State.start:
                SpawningState();
                break;
            case State.retreat:
                RetreatState();
                break;
            case State.attackRandom:
                RandomAttackState();
                break;
            case State.attackFull:
                FullAttackState();
                break;
            case State.idle:
                IdleState();
                break;
        }

        
        //Debug.Log("Distance: " + currentDistance);
        //Debug.Log("Moving Forward: " + movingForward);
        //Debug.Log("Current State: " + currentState);
    }



    //------------------------------------------------------------------------------------------------
    //States
    //------------------------------------------------------------------------------------------------


    //Used when the train is spawning, forces cannons not to fire or be present
    void SpawningState() {

        MoveForward();
        Move(false);

        if (currentDistance > farthestAway) {
            //Debug.Log("Spawning State");
            foreach (PirateCannon pc in cannonControllers)
            {
                pc.DisableCannon();
                pc.canFire = false;
            }
            EnableRandomCannons(3);
            currentState = State.attackRandom;
            SetFireAtWill(true);
        }
    }

    //Lets all cannons fire at will, with random variation
    void RandomAttackState() {
        Move(true);

        timeToFireAll -= TimeSpeed.TIMESPEED.timeChange;
        if (timeToFireAll <= 0.0f) {
            currentState = State.attackFull;
            SetFireAtWill(false);
        }

        if (cannonsEnabled < TrainAIManager.Master.difficulty)
        {
            EnableRandomCannons(1);
        }
    }

    //When all cannons have been knocked out, the train retreats for a time
    void RetreatState() {
        MoveBack();
        Move(false);

        if (currentDistance <= idleDistance) {
            currentState = State.idle;
        }
    }

    //Waits for all the cannons to be ready to fire, then fires all at once
    void FullAttackState() {
        Move(true);

        bool canAllFire = true;

        //---------------------------------------------
        //If the cannon is enabled and not broken, it should count towards the canAllFire.
        //If those cannons ^^ are all reloaded, then it fires all of them at once
        //---------------------------------------------

        for (int i = 0; i < cannonControllers.Length; i++)
        {
            if (cannonControllers[i].disabled == false && cannonControllers[i].broken == false) {
                if (cannonControllers[i].canFire == false)
                {
                    canAllFire = false;
                }
            }
        }

        if (canAllFire) {
            FireAllCannons();
            currentState = State.attackRandom;
            timeToFireAll = averageTimeToFireAll + Random.Range(-fireAllVariation, fireAllVariation);
            SetFireAtWill(true);
        }
    }

    //Waits for the Game to restart at a safe distance
    void IdleState()
    {

    }


    //------------------------------------------------------------------------------------------------
    //Functions
    //------------------------------------------------------------------------------------------------

    

    //Should be called only when all cannons can fire
    void FireAllCannons() {
        for (int i = 0; i < cannonControllers.Length; i++)
        {
            cannonReloadTime = cannonReloadTime / TrainAIManager.Master.difficulty;
            cannonControllers[i].Fire();
        }
    }

    //Changes the fireAtWill variable of each cannon
    void SetFireAtWill(bool swapTo) {
        for (int i = 0; i < cannonControllers.Length; i++)
        {
            cannonControllers[i].fireAtWill = swapTo;
        }
    }



    //Move the train around yay
    void Move(bool isRandom) {

        if (isRandom)
        {
            //Random change in direction
            if (Random.Range(0, 120) <= 1)
            {
                movingForward = !movingForward;
            }


            if (movingForward)
            {
                MoveForward();
            }
            else
            {
                MoveBack();
            }
        }

        Vector3 movingTo = transform.position;
        movingTo.z -= currentMoveSpeed * TimeSpeed.TIMESPEED.timeChange;
        transform.position = movingTo;
    }


    void MoveBack()
    {
        currentMoveSpeed += moveSpeedChange;
        if (currentMoveSpeed > maxMoveSpeed) { currentMoveSpeed = maxMoveSpeed; }

        if (currentDistance < -farthestAway)
        {
            movingForward = true;
        }
    }

    void MoveForward()
    {
        currentMoveSpeed -= moveSpeedChange;
        if (currentMoveSpeed < -maxMoveSpeed) { currentMoveSpeed = -maxMoveSpeed; }

        if (currentDistance > farthestAway)
        {
            movingForward = false;
        }
    }

    //Should only be called by the TrainAIManager script
    public void BeginGame()
    {
        currentState = State.start;
        cannonsEnabled = 0;
    }

    public void EndGame()
    {
        currentState = State.retreat;

        foreach (PirateCannon pc in cannonControllers)
        {
            pc.DisableCannon();
            pc.canFire = false;
        }
    }

    void EnableRandomCannons(int numEnable)
    {
        //Debug.Log("Attempting to Enable: " + numEnable + " Cannons");
        for (int i = 0; i <= numEnable; i++)
        {
            
            PirateCannon pc = cannonControllers[Random.Range(0, cannonControllers.Length)];
            if (pc.disabled == true)
            {
                pc.EnableCannon();
                cannonsEnabled += 1;
                //Debug.Log("Cannon Enabled");
            }
            
        }
    }
}
