using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public bool startButton;
    public bool quitButton;
    public bool highscoreButton;

    public bool startHidden;

    public GameObject[] menuItems;
    private Vector3[] menuItemLocations;
    private Vector3[] menuItemRotations;

    private bool hasHidden;

    private void Start() {
        menuItemLocations = new Vector3[menuItems.Length];
        menuItemRotations = new Vector3[menuItems.Length];

        for (int i = 0; i < menuItems.Length; i++) {
            menuItemLocations[i] = menuItems[i].transform.position;
            menuItemRotations[i] = menuItems[i].transform.rotation.eulerAngles;
        }
    }

    private void Update()
    {
        if (startHidden & !hasHidden)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 100.0f, transform.position.z);
            hasHidden = true;
        }
    }



    //Occurs when button is pressed
    public void ButtonShot() {
        Debug.Log("Button Press Occurred");
        if (quitButton) {
            Application.Quit();
        }


        if (startButton) {
            //Debug.Log("Start Button");
            BlastObjects();
            TrainAIManager.Master.StartGame();
        }


        if (highscoreButton)
        {
            Debug.Log("Reset Main Menu");
            
            BlastObjects();
            TrainAIManager.Master.ResetMenu();
        }
    }

    public void ResetMenu() {
        for (int i = 0; i < menuItems.Length; i++) {
            menuItems[i].GetComponent<BoxCollider>().enabled = true;
            menuItems[i].GetComponent<Rigidbody>().isKinematic = true;
            menuItems[i].transform.position = menuItemLocations[i];
            menuItems[i].transform.rotation = Quaternion.Euler(menuItemRotations[i]);

            if (menuItems[i].GetComponent<HighScoreTable>() != null)
            {
                menuItems[i].GetComponent<HighScoreTable>().UpdateTable();
            }
        }
    }

    //Sends objects flying
    void BlastObjects()
    {
        foreach (GameObject item in menuItems)
        {
            item.GetComponent<Rigidbody>().isKinematic = false;
            item.GetComponent<BoxCollider>().enabled = false;
        }
        this.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * -1000);

    }
}
