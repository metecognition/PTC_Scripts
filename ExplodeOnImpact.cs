using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnImpact : MonoBehaviour
{
    public GameObject explosionEffect;
    public GameObject scoreEffect;
    public GameObject goldFlyingEffect;


    private void OnCollisionEnter(Collision collision) {
        GameObject ef = Instantiate(explosionEffect);       //ef stands for explosion effect
        ef.transform.position = transform.position;

        //check if object is a button here
        if (collision.gameObject.CompareTag("MenuButtons") || collision.gameObject.CompareTag("HighscoreMenu")) {
            collision.gameObject.GetComponent<MenuButton>().ButtonShot();
            Debug.Log("Button Shot");
        }


        //--------------------------------------------------------------------------------------
        //If Player Shot, then give score increase and multiplier
        //--------------------------------------------------------------------------------------
        if (this.gameObject.CompareTag("Player Shot") && GetComponent<ExplosiveCannonBallAOE>() == null)
        {
            if (collision.gameObject.CompareTag("PirateCannon"))
            {
                ScoreMoneyManager.S.IncreaseScore();


                if (scoreEffect != null)// && collision.gameObject.GetComponent<MeshRenderer>().enabled == true)           //Replace when animation for cannon is added
                {
                    GameObject sf = Instantiate(scoreEffect);
                    sf.GetComponent<ScorePopup>().scoreDisplay = ScoreMoneyManager.S.score;
                    sf.GetComponent<ScorePopup>().multiplierDisplay = ScoreMoneyManager.S.multiplier;
                    sf.GetComponent<ScorePopup>().DisplayScore();
                    sf.transform.position = transform.position;
                }

                ScoreMoneyManager.S.multiplier *= 2;

            }
            else if (!collision.gameObject.CompareTag("PowerUp")  && CannonAim.S.currentPowerUp != 2)
            {
                ScoreMoneyManager.S.multiplier = 1;
                //Debug.Log("Setting Multiplier to 1, Object " + gameObject + " collided with : " + collision.gameObject);
            }
        }


        //--------------------------------------------------------------------------------------
        //If this is a pirate cannonball and it hits gold, send gold flying
        //--------------------------------------------------------------------------------------
        if (collision.gameObject.CompareTag("Gold"))
        {
            if (goldFlyingEffect != null)
            {
                GameObject gef = Instantiate(goldFlyingEffect);
                gef.transform.position = transform.position;
            }
        }

        Destroy(gameObject);
    }
}
