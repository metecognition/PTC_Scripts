using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainAIManager : MonoBehaviour
{
    static public TrainAIManager Master;

    [Header("!!!   SINGLETON   !!!")]
    public float difficulty = 4.0f;
    public GameObject trainLeft;
    public GameObject trainRight;

    [Header("Timer Variables")]
    public float gameLength = 50.0f;
    private float gameTimer;

    private TrainAI leftAI;
    private TrainAI rightAI;
    private bool isGamePlaying = false;
    private float startDifficulty;

    private AudioSource MusicSource;
    void Start()
    {
        if (Master == null) {
            Master = this;
        }
        else {
            Debug.Log("Attempted to assign second singleton: TrainAIManager");
        }

        leftAI = trainLeft.GetComponent<TrainAI>();
        rightAI = trainRight.GetComponent<TrainAI>();

        MusicSource = GetComponent<AudioSource>();

        startDifficulty = difficulty;
    }

    private void Update()
    {
        if (isGamePlaying)
        {
            gameTimer += TimeSpeed.TIMESPEED.timeChange;

            if (gameTimer > gameLength)
            {
                ResetGame();
                gameTimer = 0.0f;
            }
        }
    }

    public void StartGame()
    {
        leftAI.BeginGame();
        rightAI.BeginGame();

        MusicSource.Play();
        isGamePlaying = true;
        ScoreMoneyManager.S.score = 0.0f;
    }

    public void ResetGame()
    {
        leftAI.EndGame();
        rightAI.EndGame();

        //ResetMenu();
        ScoreMoneyManager.S.SubmitScore();
        ScoreMoneyManager.S.ResetMoney();

        DisplayHighscores();

        MusicSource.Stop();
        isGamePlaying = false;

        difficulty = startDifficulty;
    }

    public void DisplayHighscores()
    {
        GameObject[] highscoreMenu = GameObject.FindGameObjectsWithTag("HighscoreMenu");
        foreach (GameObject i in highscoreMenu)
        {
            i.GetComponent<MenuButton>().ResetMenu();
        }
    }

    public void ResetMenu()
    {
        GameObject[] menuItems = GameObject.FindGameObjectsWithTag("MenuButtons");
        foreach (GameObject i in menuItems)
        {
            i.GetComponent<MenuButton>().ResetMenu();
        }
    }
}
