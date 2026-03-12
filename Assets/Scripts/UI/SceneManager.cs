
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;


public class sceneManager_ : MonoBehaviour
{
    public int currentScene;
    public int newScene;

    public string sceneName;

    public animations animScript;


    public GameObject pausePanel;
    public GameObject settingsPanel;

    public bool _questMenuOpen = false;
    public GameObject _questMenu;
    bool paused = false;
     bool canPause = true; 

    public void Start()
    {
        if (GameObject.Find("QuestManager") == null) //lmao skill issue
        {
            SceneManager.LoadScene("MAIN_Menu");
        }
    }


    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            OpenQuestMenu();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (_questMenuOpen)
            {
                OpenQuestMenu(); //Closes the quest menu if it's open
            }
            else
            {
                paused = !paused; //Otherwise pauses
                pauseGame();
            }
        }
    }

    public void returnToMain()
    {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
    public void loadLastScene()
    {
        StartCoroutine(loadBack());
        Debug.Log("Current Build index is " + SceneManager.GetActiveScene().buildIndex);

    }
    //temp
    public void loadShipScene()
    {
        StartCoroutine(loadShip());
        Debug.Log("Current Build index is " + SceneManager.GetActiveScene().buildIndex);

    }
    //temp
    public void loadSpaceScene()
    {
        StartCoroutine(loadSpace());
        Debug.Log("Current Build index is " + SceneManager.GetActiveScene().buildIndex);

    }
    public void exitGame()
    {
        Debug.Log("Exiting!");
        Application.Quit();
    }
    public void loadSettings()
    {
        //if we want time to pause in settings
        Time.timeScale = 0.0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

       
        settingsPanel.SetActive(true);
        Debug.Log("Loading Settings");

    }
    public void closeSettings()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
        resetTime();
    }
    public void resetTime()
    {
        Time.timeScale = 1.0f;
    }

    public void resetScene()
    {
        Debug.Log("ResettingScene");
        //for starting the game over again
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void pauseGame()
    {
     checkIfSettingActive();
        Debug.Log(paused);
        if (paused&&canPause)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0.0f;
            Cursor.visible = true;
            pausePanel.SetActive(true);
        }
        if (!paused&&!canPause)
        {
            settingsPanel.SetActive(false);
            Time.timeScale = 1.0f;
            pausePanel.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }else if (!paused)
        {
            Time.timeScale = 1.0f;
            pausePanel.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        //if (settingsPanel.activeSelf)
        //{
        //paused = !paused;
        //}
    }
    public void OpenQuestMenu()
    {
        if (_questMenuOpen)
        {
            _questMenu.SetActive(false);
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            _questMenu.SetActive(true);
            _questMenu.GetComponent<QuestListSetter>().SetQuests();
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        _questMenuOpen = !_questMenuOpen;
    }
    IEnumerator loadIn()
    {
        animScript.anim_.Play("fadeIn",0,0);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
         

    }
    IEnumerator loadBack()
    {
        animScript.anim_.Play("fadeIn", 0, 0);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);


    }
    //temp
    IEnumerator loadShip()
    {
        animScript.anim_.Play("fadeIn", 0, 0);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("MAIN_InsideShip");


    }
    //temp
    IEnumerator loadSpace()
    {
        animScript.anim_.Play("fadeIn", 0, 0);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("MAIN_GameScene");


    }
    IEnumerator returing()
    {
        animScript.anim_.Play("fadeIn", 0, 0);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(0);


    }

    void checkIfSettingActive()
    {
        //temp fix-- will rewrite the pause code later
        if (settingsPanel.activeInHierarchy)
        {
            canPause = false;
            Debug.Log("Settings panel active");
           
        }
        else
        {
            canPause = true;
            Debug.Log("Settings panel inactive");




        }
    }
}
