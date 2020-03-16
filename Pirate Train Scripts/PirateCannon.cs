using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateCannon : MonoBehaviour
{
    [Header("Set by TrainAI script")]
    public GameObject cannonBall;
    public bool canFire = false;
    public float reloadTime = 2.0F;
    public float randomReloadVariation = 1.0F;
    public bool broken = false;
    public bool disabled = true;
    public bool fireAtWill = false;
    public float difficultyIncrease;
    public GameObject cannonFireEffect;
    public float repairTime = 20.0f;
    public AudioClip[] cannonSounds;
    public Transform cannonBallSpawnPoint;

    //private MeshRenderer mr;
    public GameObject cannonDoor;
    public GameObject cannonBody;
    private CapsuleCollider cc;
    private BoxCollider bc;
    private float reloadTimeRemaining;
    private float repairTimeRemaining;
    private AudioSource cannonSoundSource;


    private void Start() {
        //mr = GetComponent<MeshRenderer>();
        cc = GetComponent<CapsuleCollider>();
        bc = GetComponent<BoxCollider>();
        
        DisableCannon();

        cannonSoundSource = GetComponent<AudioSource>();

        reloadTimeRemaining = 0.5f;
    }


    //-----------------------------------------------------------------------------
    //Fire Fuction, Can Only Fire if Loaded, Not Broken, and Not Disabled
    //-----------------------------------------------------------------------------
    public void Fire() {

        if (canFire && !broken && !disabled) {
            GameObject i = Instantiate(cannonBall);
            i.transform.rotation = transform.rotation;
            i.transform.position = cannonBallSpawnPoint.position;
            i.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * (-2000.0f + Random.Range(-500.0f, 500.0f)));
            reloadTimeRemaining = reloadTime + Random.Range(-randomReloadVariation, randomReloadVariation);
            canFire = false;

            GameObject ef = Instantiate(cannonFireEffect);
            ef.transform.rotation = transform.rotation;
            ef.transform.position = cannonBallSpawnPoint.position;

            PlaySound();
        }
    }

    private void Update() {
        if (!broken && !disabled)
        {
            reloadTimeRemaining -= TimeSpeed.TIMESPEED.timeChange;
        }

        if (reloadTimeRemaining <= 0) {
            canFire = true;
        }

        //Shoots randomly if cannon can fire at will
        if (canFire && fireAtWill) {
            Fire();
        }

        if (broken)
        {
            repairTimeRemaining -= TimeSpeed.TIMESPEED.timeChange;
            if (repairTimeRemaining <= 0.0f)
            {
                FixCannon();
            }
        }
    }


    //Check collision with player cannon ball
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player Shot" && disabled == false) {
            BreakCannon();
        }
    }



    //--------------------------------------------------------------------
    //Break and Fix Cannon Fuctions, Controlled By This Script
    //--------------------------------------------------------------------
    public void BreakCannon() {
        broken = true;
        CloseDoor();
        cc.enabled = false;
        bc.enabled = false;
        TrainAIManager.Master.difficulty += difficultyIncrease;
        repairTimeRemaining = repairTime;
    }

    public void FixCannon()
    {
        broken = false;
        if (!disabled)
        {
            OpenDoor();
            cc.enabled = true;
            bc.enabled = true;
        }
    }


    //--------------------------------------------------------------------
    //Disable and Enable Cannon Fuctions, Controlled By State Machine
    //--------------------------------------------------------------------
    public void DisableCannon()
    {
        disabled = true;
        CloseDoor();
        cc.enabled = false;
        bc.enabled = false;
    }

    public void EnableCannon()
    {
        disabled = false;
        OpenDoor();
        cc.enabled = true;
        bc.enabled = true;
    }

    void PlaySound()
    {
        cannonSoundSource.clip = cannonSounds[Random.Range(0, cannonSounds.Length)];
        cannonSoundSource.PlayDelayed(Random.Range(0.0f, 0.2f));
    }

    void CloseDoor()
    {
        cannonDoor.transform.localEulerAngles = new Vector3(185.0f, 0.0f, 0.0f);
        cannonBody.GetComponent<MeshRenderer>().enabled = false;
    }

    void OpenDoor()
    {
        cannonDoor.transform.localEulerAngles = new Vector3(-10.0f, 0.0f, 0.0f);
        cannonBody.GetComponent<MeshRenderer>().enabled = true;
    }
}
