using UnityEngine;

public class setting_ : MonoBehaviour
{
    public GameObject settingPanel;
    public bool inGame=false;
    




    public void adjustVolume()
    {
        //empty for now as we have no sound
    }

    public void adjustResolution()
    {
        //placeholder method
    }

    public void adjustMouseSensetivity()
    {
        //dont know if i will implement this method yet 
    }

    public void showSettings()
    {

        Time.timeScale = 0.0f;
        settingPanel.SetActive(true);
    }
    public void hideSettings()
    {
        Time.timeScale = 1.0f;
        settingPanel.SetActive(false);
    }
}
