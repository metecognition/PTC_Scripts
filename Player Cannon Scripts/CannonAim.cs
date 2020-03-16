using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;
using TMPro;

public class CannonAim : MonoBehaviour
{
    static public CannonAim S;

    [Header("!!!    Singleton    !!!")]
    public Transform lookingAt;

    public GameObject cannonBall;
    public float reloadSpeed = 0.5f;
    public GameObject cannonBallSpawn;

    //for currentPowerUp, 0=normal, 1=explosive, 2=rapid
    public int currentPowerUp;
    public float powerUpTime;

    public GameObject fireEffect;

    [Header("Variables for Explosive Shot")]
    public float damageRadius = 2.0f;
    public GameObject explosiveCannonBall;

    [Header("Variables for Rapid Shot")]
    public float rapidFireSpeed = 0.25f;

    private bool hasFired = false;
    private float reloadTimer = 0.0f;

    [Header("Powerup Display Controls")]
    public MeshRenderer explosiveDisplay;
    public MeshRenderer rapidDisplay;
    public TextMeshPro powerUpTimeDisplay;

    [Header("Cannon Sounds")]
    public AudioClip[] cannonSounds;

    private AudioSource cannonSoundSource;

    [Header("Shot Vibration")]
    public bool shouldVibrate = true;
    public float vibrationIntensity = 0.5f;
    public float vibrationDuration = 0.1f;


    private void Start()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.Log("Attempted to assign second Singleton CannonAim.S");
        }

        cannonSoundSource = cannonBallSpawn.GetComponent<AudioSource>();
    }

    void Update()
    {
        //rotate cannon
        transform.LookAt(lookingAt);
        OVRInput.Update();

        //Debug.Log(OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger));


        //fire cannon balls here
        if (currentPowerUp == 0 || currentPowerUp == 1) {
            NormalFire();
        }
        else {
            RapidFire();
        }

        reloadTimer -= TimeSpeed.TIMESPEED.timeChange;

        //checks time on powerups and if they should be still active
        if (currentPowerUp != 0) {
            powerUpTime -= TimeSpeed.TIMESPEED.timeChange;
            if (powerUpTime <= 0) {
                currentPowerUp = 0;
            }
            powerUpTimeDisplay.enabled = true;
            powerUpTimeDisplay.text = (((int)powerUpTime)+1).ToString();

            //Active powerup displays
            if (currentPowerUp == 1)
            {
                explosiveDisplay.enabled = true;
                rapidDisplay.enabled = false;
            }
            else if (currentPowerUp == 2)
            {
                rapidDisplay.enabled = true;
                explosiveDisplay.enabled = false;
            }
        }
        else
        {
            powerUpTimeDisplay.enabled = false;
            rapidDisplay.enabled = false;
            explosiveDisplay.enabled = false;
        }

        
    }

    //Normal Fire
    //Also used for explosive shot
    void NormalFire() {
        if (lookingAt.GetComponent<CannonPointTrack>().canFire == true) {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.9f || OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.9f) {  //checks if triggers pressed
                if (!hasFired && reloadTimer < 0.0f) { //checks if can fire again

                    StartCoroutine(ShotVibrate());

                    hasFired = true;
                    reloadTimer = reloadSpeed;

                    //fire here
                    GameObject ball;
                    if (currentPowerUp == 0) {
                        ball = Instantiate(cannonBall);
                        ball.transform.position = cannonBallSpawn.transform.position;
                        ball.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * -3000);
                    }
                    else if (currentPowerUp == 1) {
                        ball = Instantiate(explosiveCannonBall);
                        ball.transform.position = cannonBallSpawn.transform.position;
                        ball.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * -3000);
                    }

                    FireCannonEffect();
                }
            }
            else {
                hasFired = false;
            }
        }
    }

    //Rapid fire
    //Doesn't check to see if triggers have been released, and uses slower reload time
    void RapidFire() {
        if (lookingAt.GetComponent<CannonPointTrack>().canFire == true) {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.9f || OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.9f) {
                if (reloadTimer <= 0.0f) {

                    StartCoroutine(ShotVibrate());

                    reloadTimer = rapidFireSpeed;

                    //instantiate cannon ball
                    GameObject ball = Instantiate(cannonBall);
                    ball.transform.position = cannonBallSpawn.transform.position;
                    ball.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * -3000);

                    FireCannonEffect();
                }
            }
        }
    }

    //Spawn cannon fire effect at end of barrel
    void FireCannonEffect() {
        GameObject ef = Instantiate(fireEffect);
        ef.transform.position = cannonBallSpawn.transform.position;
        ef.transform.rotation = transform.rotation;
        cannonSoundSource.clip = cannonSounds[Random.Range(0, cannonSounds.Length)];
        cannonSoundSource.Play();
    }

    IEnumerator ShotVibrate()
    {
        if (shouldVibrate)
        {
            OVRInput.SetControllerVibration(vibrationIntensity / 2, vibrationIntensity, OVRInput.Controller.RTouch);
            OVRInput.SetControllerVibration(vibrationIntensity / 2, vibrationIntensity, OVRInput.Controller.LTouch);
            yield return new WaitForSeconds(vibrationDuration);
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        }
    }
}
