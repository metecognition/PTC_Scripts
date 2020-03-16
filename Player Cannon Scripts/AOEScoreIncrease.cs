using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEScoreIncrease : MonoBehaviour
{
    bool hasDestroyedCannon = false;
    float cannonsDestroyed = 0.0f;

    public GameObject scoreEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PirateCannon"))
        {
            cannonsDestroyed += 1;
            hasDestroyedCannon = true;

            //Debug.Log("AOE Cannon Destroyed");
        }
    }

    private void OnDestroy()
    {
        if (hasDestroyedCannon)
        {
            for (int i = 0; i < cannonsDestroyed; i++)
            {
                ScoreMoneyManager.S.IncreaseScore();
                ScoreMoneyManager.S.multiplier *= 2;
            }

            GameObject sf = Instantiate(scoreEffect);
            sf.GetComponent<ScorePopup>().scoreDisplay = ScoreMoneyManager.S.score;
            sf.GetComponent<ScorePopup>().multiplierDisplay = ScoreMoneyManager.S.multiplier;
            sf.GetComponent<ScorePopup>().DisplayScore();
            sf.transform.position = transform.position;
        }
    }
}
