using UnityEngine;

public class setting_ : MonoBehaviour
{
    public GameObject settingPanel;
    //public
   public bool inGame=false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
