using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScoreMoneyManager : MonoBehaviour
{
    static public ScoreMoneyManager S;

    [Header("!!!   SINGLETON   !!!")]
    public float moneyLost = 0.0f;
    public float startMoney = 1000.0f;

    public float score = 0.0f;
    public int multiplier = 1;

    [Header("Score Amounts")]
    public float scorePerCannonDestroyed = 1.0f;

    [Header("Max Multiplier (2, 4, 8, 16...")]
    public int maxMultiplier = 32;

    [Header("Score Saver Object")]
    public ScoreSaver sc;


    private void Start()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.Log("Attempted to assign second Singleton: SCoreMoneyManager.S");
        }

        sc.Load();

    }
    private void Update()
    {
        if (moneyLost > startMoney)
        {
            TrainAIManager.Master.ResetGame();
        }

        //Debug.Log("Score: " + score + ", Multiplier: " + multiplier);
    }

    public void LoseMoney(float amountLost)
    {
        moneyLost += amountLost;
    }

    public void ResetMoney()
    {
        moneyLost = 0.0f;
    }

    public void IncreaseScore(bool isCannonDestroyed = true)
    {
        if (isCannonDestroyed)
        {
            if (multiplier > maxMultiplier) { multiplier = maxMultiplier; }
            score += scorePerCannonDestroyed * multiplier;
        }
    }

    public void SubmitScore()
    {
        sc.SubmitHighScore(Mathf.RoundToInt(score));
        sc.Save();
    }

}
