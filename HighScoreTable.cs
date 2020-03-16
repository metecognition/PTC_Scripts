using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreTable : MonoBehaviour
{
    public TextMeshPro CurrentScore;

    public TextMeshPro[] HighscoreDates;
    public TextMeshPro[] Highscores;

    public ScoreSaver ScoreSaveMaster;

    public void UpdateTable()
    {
        for(int i = 0; i<ScoreSaveMaster.HighScoresCount; i++)
        {
            Highscores[i].text = ScoreSaveMaster.HighScore(i).ToString();
            HighscoreDates[i].text = ScoreSaveMaster.HighScoreDateTime(i);
        }

        CurrentScore.text = ScoreMoneyManager.S.score.ToString();
    }

    private void Start()
    {
        StartCoroutine(UpdateTableAfterWait());
    }

    //Done to stop problems caused by trying to access scriptable object loading json at runtime
    IEnumerator UpdateTableAfterWait()
    {
        yield return new WaitForSeconds(1.0f);
        UpdateTable();
    }
}
