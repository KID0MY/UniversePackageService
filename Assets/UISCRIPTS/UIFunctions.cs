using UnityEngine;
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

    

   

    public void gameOver()
    {
        if (isOver)
        {
            popUpPanel.SetActive(isOver);
            panelText.text = ("Woah! You've collected " + numofCollected + " packages. Your highscore is " + highestNum + ". Would you like to try again and beat it?"); 
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
    
}
