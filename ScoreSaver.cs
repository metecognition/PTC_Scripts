using UnityEngine;
using System.IO;

[CreateAssetMenu(menuName = "ScriptableObjects/ScoreSaver", order = 1)]
public class ScoreSaver : ScriptableObject
{
    //-----------------------------------------------------------
    //Save Variables
    //-----------------------------------------------------------
    [SerializeField]
    private int highScoresCount = 5;

    private string filePath;
    [SerializeField]
    private string fileName = "highscores";
    [SerializeField]
    private int[] highScores;
    [SerializeField]
    private string[] highScoreDates;
    [SerializeField]
    private string[] highScoreTimes;


    //-----------------------------------------------------------------------
    //Save Functions
    //-----------------------------------------------------------------------

    private string FilePath
    {
        get
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = Path.Combine(Application.persistentDataPath, fileName);

            }
            return filePath;
        }
    }

    public int HighScoresCount
    {
        get { return highScoresCount; }
    }

    public int HighScore(int i)
    {
        return highScores[i];
    }
    public string HighScoreDateTime(int i)
    {
        return highScoreDates[i] + " - " + highScoreTimes[i];
    }

    public float this[int i]
    {
        get { return highScores[i]; }
    }

    public void SubmitHighScore(int highScore = -1)
    {
        Debug.Log("Attempting to submit highscore: " + highScore);
        if (highScores == null)
        {
            highScores = new int[HighScoresCount];
        }

        int newHighScoreIndex = -1;

        for (int i = 0; i < highScores.Length; i++)
        {
            if (highScore >= highScores[i])
            {
                newHighScoreIndex = i;
                break;
            }
        }

        if (newHighScoreIndex >= 0)
        {
            System.DateTime theTime = System.DateTime.Now;
            string date = theTime.Month + "/" + theTime.Day + "/" + theTime.Year;
            string time = theTime.Hour + ":" + theTime.Minute + " AM";
            if (theTime.Hour > 12)
            {
                time = (theTime.Hour - 12) + ":" + theTime.Minute + " PM";
            }
            for (int i = highScores.Length - 1; i > newHighScoreIndex; i--)
            {
                //Debug.Log(highScores[i]);
                //Debug.Log(highScores[i - 1]);
                highScores[i] = highScores[i - 1];
                highScoreDates[i] = highScoreDates[i - 1];
                highScoreTimes[i] = highScoreTimes[i - 1];
            }
            highScores[newHighScoreIndex] = highScore;
            highScoreDates[newHighScoreIndex] = date;
            highScoreTimes[newHighScoreIndex] = time;

        }
        else
        {
            Debug.Log("Failed to set high score, score too low.");
        }

        Save();
    }


    public void Save()
    {
        File.WriteAllText(FilePath, JsonUtility.ToJson(this));
    }

    public void Load()
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        Debug.Log("File " + fileName + " stored at " + Application.persistentDataPath);
        if (File.Exists(filePath))
        {
            Debug.Log("Loaded JSON");
            JsonUtility.FromJsonOverwrite(File.ReadAllText(filePath), this);
            Debug.Log(highScores);
        }
        else if (highScores.Length < highScoresCount)
        {
            Debug.Log("Created new high scores");
            highScores = new int[highScoresCount];
            System.DateTime theTime = System.DateTime.Now;
            string date = theTime.Month + "/" + theTime.Day + "/" + theTime.Year;
            string time = theTime.Hour + ":" + theTime.Minute + " AM";
            if (theTime.Hour > 12)
            {
                time = (theTime.Hour - 12) + ":" + theTime.Minute + " PM";
            }

            highScoreDates = new string[highScoresCount];
            highScoreTimes = new string[highScoresCount];
            for (int i = 0; i < highScores.Length; i++)
            {
                highScoreDates[i] = date;
                highScoreTimes[i] = time;
            }

            Save();

        }
    }
}
