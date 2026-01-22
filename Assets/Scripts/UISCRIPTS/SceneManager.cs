
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class sceneManager_ : MonoBehaviour
{
    public int currentScene;
    public int previousScene; 
    public int newScene;

    public string sceneName;

    public animations animScript;
    public GameObject pausePanel;

    bool paused=false;
    public setting_ settingScript;

    private void Start()
    {
        currentScene= SceneManager.GetActiveScene().buildIndex;
    }


    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            paused = !paused;
                pauseGame();
            
        }
    }
    public void returnToMain()
    {
        StartCoroutine(returing());

        Debug.Log("Current Build index is " + SceneManager.GetActiveScene().buildIndex);
    }

    public void loadNextScene()
    {
        //animScript.fadeOutAnim();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        StartCoroutine(loadIn());
        Debug.Log("Current Build index is " + SceneManager.GetActiveScene().buildIndex);

    }
    public void exitGame()
    {
        Debug.Log("Exiting!");
        Application.Quit();
    }
    public void loadSettings()
    {

        if (SceneManager.GetActiveScene().name != "Main_Settings")
        {
            settingScript.inGame = true;
        }
        if (settingScript.inGame==true)
        {
          //  Time.timeScale = 0.0f;
            settingScript.showSettings();
            Debug.LogWarning("1");
        }
        else if (settingScript.inGame == false)
        {

            savePreviousScene();
            SceneManager.LoadScene("MAIN_Settings");
            Debug.LogWarning("2");


        }

    }
    public void returnToScene()
    {
        SceneManager.LoadScene(previousScene);
    }
    public void resetScene()
    {
        Debug.Log("ResettingScene");
        //for starting the game over again
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void pauseGame()
    {
        if (paused)
        {
            Cursor.visible = true;
            Time.timeScale = 0.0f;
            pausePanel.SetActive(true);
        }
        if (!paused)
        {
            Cursor.visible = false;
            Time.timeScale = 1.0f;
            pausePanel.SetActive(false);

        }
    }
    void savePreviousScene()
    {
        previousScene=currentScene;
    }
    IEnumerator loadIn()
    {
        animScript.anim_.Play("fadeIn",0,0);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
         

    }
    IEnumerator returing()
    {
        animScript.anim_.Play("fadeIn", 0, 0);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(0);


    }
    
}
