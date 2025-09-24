using UnityEngine;
using System.Collections;

using TMPro; 
public class UIFunctions : MonoBehaviour
{

    public GameObject popUpPanel;
    public bool isOver;
    public float speed;
    public TMP_Text panelText;
    public TMP_Text updateItemText;
    public int numofCollected=0;
    public int highestNum=0;
    public Animator anim; 
    public SpaceshipControl spaceship;
    

   

    public void gameOver()
    {
        if (isOver)
        {
            Cursor.lockState = CursorLockMode.None;
            popUpPanel.SetActive(isOver);
            spaceship.enabled = false;
            StartCoroutine(flashBox());
            panelText.text = ("Woah! You've collected " + numofCollected + " packages. Your highscore is " + highestNum + ". Would you like to try again and beat it?");
            StopCoroutine(flashBox());
        }
        else
        {
            isOver = false;
            popUpPanel.SetActive(false);
        }
    }

    public void SetCountDown()
    {

    }
    public void showSpeed()
    {

    }
    void highScoreTrack()
    {
        if (isOver)
        {
            if (numofCollected > highestNum) {
                highestNum = numofCollected;
                    }
            else
            {

                highestNum = highestNum;
            }
        }
    }

    void updateItemsCollectedUI()
    {
        //call everytime a package is picked up
        updateItemText.text = "Packages Collected: " + numofCollected;
    }
     IEnumerator flashBox()
    {
        anim.SetBool("gameOver", true);
        yield return new WaitForSeconds(2.0f);
        anim.SetBool("gameOver", false);
      anim.SetBool("end",true);

    }
}
