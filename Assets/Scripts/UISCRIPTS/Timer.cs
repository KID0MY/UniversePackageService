using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeRemaining;
    public TMP_Text timerText;
    public UIFunctions uiFunc;
    private bool timerStarted;

    private void Start()
    {
        uiFunc = GetComponent<UIFunctions>();
        timerStarted = true;
    }


    private void Update()
    {
        if (timerStarted) checkTimer();
    }


    private void checkTimer()
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

    private void updateTimer(float currentTime)
    {
        currentTime += 1;
        float mins = Mathf.FloorToInt(currentTime / 60);
        float secs = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00} : {1:00}", mins, secs);
    }

    private void resetTimer()
    {
        timerStarted = false;
        timeRemaining = 20; //whatever the times ends up being
    }
}