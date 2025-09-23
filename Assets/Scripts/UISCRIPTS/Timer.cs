using UnityEngine;
using TMPro; 
public class Timer : MonoBehaviour
{
    public float timeRemaining;
    public TMP_Text timerText;
    public UIFunctions uiFunc;
    bool timerStarted;
    void Start()
    {
        uiFunc = GetComponent<UIFunctions>();
        timerStarted = true;
    }


    void Update()
    {
        if (timerStarted) { 
        checkTimer();
    }
    }


    void checkTimer()
    {
        if (timerStarted)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                updateTimer(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                uiFunc.isOver = true;

                uiFunc.gameOver();
            }
        }
    }
    void updateTimer(float currentTime)
    {
        currentTime += 1;
        float mins = Mathf.FloorToInt(currentTime / 60);
        float secs = Mathf.FloorToInt(currentTime %60);
        timerText.text = string.Format("{0:00} : {1:00}",mins,secs);
    }
    void resetTimer()
    {
        timerStarted = false;
        timeRemaining = 20; //whatever the times ends up being

    }
}



