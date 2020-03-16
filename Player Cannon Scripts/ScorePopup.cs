using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    public TextMeshPro scoreArea;
    public TextMeshPro multiplierArea;
    public float verticalSpeed = 1.0f;
    public float displayTime = 1.5f;
    public bool shouldFade = true;
    public float moveSpeedMultiplier = 0.4f;


    [Header("Change only via script")]
    public float scoreDisplay;
    public float multiplierDisplay;

    private Transform target;
    private bool hasOffset;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void DisplayScore()
    {
        scoreArea.text = scoreDisplay.ToString();
        if (multiplierDisplay > 2.0f)
        {
            multiplierArea.text = "x" + (multiplierDisplay).ToString();
        }
        else
        {
            multiplierArea.text = "";
        }
    }

    private void Update()
    {
        displayTime -= TimeSpeed.TIMESPEED.timeChange;
        if (displayTime <= 0.0f)
        {
            Destroy(gameObject);
        }

        if (shouldFade && displayTime < 1.0f)
        {
            scoreArea.alpha = displayTime;
            multiplierArea.alpha = displayTime;
        }

        Vector3 newPos = transform.position;
        newPos.y += verticalSpeed * TimeSpeed.TIMESPEED.timeChange;
        transform.position = newPos;

        transform.LookAt(target);
        if (!hasOffset)
        {
            transform.position = transform.position + (transform.forward/moveSpeedMultiplier);
        }
    }



}
