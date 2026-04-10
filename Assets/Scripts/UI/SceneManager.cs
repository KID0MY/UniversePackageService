
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;


public class sceneManager_ : MonoBehaviour
{
    //public int currentScene;
    public int newScene;

    public string sceneName;

    public animations animScript;


    public GameObject pausePanel;
    public GameObject settingsPanel;

    public bool _questMenuOpen = false;
    public GameObject _questMenu;
    bool paused = false;
     bool canPause = true;
    bool startGame = false;
    public int currentIndex;
    //public audioManager audioManager_;

    public void Start()
    {
        currentIndex = SceneManager.GetActiveScene().buildIndex;
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

        Debug.Log("Current Build index is " + GetCurrentSceneIndex());
    }

    public void loadNextScene()
    {
        //animScript.fadeOutAnim();
        //SceneManager.LoadScene(GetCurrentSceneIndex() + 1);
        StartCoroutine(loadIn());
        Debug.Log("Current Build index is " + GetCurrentSceneIndex());
    }
    public void loadLastScene()
    {
        StartCoroutine(loadBack());
        Debug.Log("Current Build index is " + GetCurrentSceneIndex());
    }
    //temp
    public void loadShipScene()
    {
        if (startGame == false)
        {
            StartCoroutine(loadShip());
            Debug.Log("Current Build index is " + GetCurrentSceneIndex());
            startGame = true;
        }
    }

    //temp
    public void loadSpaceScene()
    {
        StartCoroutine(loadSpace());
        Debug.Log("Current Build index is " + GetCurrentSceneIndex());

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
        SceneManager.LoadScene(GetCurrentSceneIndex());
    }

    public void pauseGame()
    {
     checkIfSettingActive();
        Debug.Log(paused);
        if (paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0.0f;
            Cursor.visible = true;
            pausePanel.SetActive(true);
            settingsPanel.SetActive(true);
        }
        if (!paused&&checkIfSettingActive())
        {
            settingsPanel.SetActive(false);
            Time.timeScale = 1.0f;
            pausePanel.SetActive(false);
            settingsPanel.SetActive(false);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            settingsPanel.SetActive(false) ;
        }else if (!paused&&checkIfSettingActive()==false)
        {
            Time.timeScale = 1.0f;
            pausePanel.SetActive(false);
           // settingsPanel.SetActive(false) ;
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
        SceneManager.LoadScene(GetCurrentSceneIndex() + 1);
        SetMusic();

    }
    IEnumerator loadBack()
    {
        animScript.anim_.Play("fadeIn", 0, 0);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(GetCurrentSceneIndex() - 1);
        SetMusic();

    }
    //temp
    IEnumerator loadShip()
    {
        animScript.anim_.Play("fadeIn", 0, 0);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("MAIN_InsideShip");
        currentIndex = 0;
        SetMusic();

    }
    //temp
    IEnumerator loadSpace()
    {
        animScript.anim_.Play("fadeIn", 0, 0);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("MAIN_SpaceScene");
        SetMusic();

    }
    IEnumerator returing()
    {
        animScript.anim_.Play("fadeIn", 0, 0);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(0);
        SetMusic();

    }
    IEnumerator loadDrasil()
    {
        animScript.anim_.Play("fadeIn", 0, 0);
        yield return new WaitForSeconds(1.0f);
        //currentIndex = 3;
        SceneManager.LoadScene("MAIN_Drasil");
        currentIndex = 3;
        SetMusic();

    }

    public void goToDrasil()
    {
        StartCoroutine(loadDrasil());   
    }
    public void goToOrbitron()
    {
        StartCoroutine (loadOrbitron());
    }
   IEnumerator loadOrbitron()
    {
        animScript.anim_.Play("fadeIn", 0, 0);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("MAIN_Orbitron");
        currentIndex = 2;
        SetMusic();

    }

    public int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void SetMusic()
    {
       // currentIndex= getCurrentIndex();
        Debug.Log("what is that melody?!");
       
        switch (currentIndex)
        {
            case 0:
                audioManager.Instance.PlayMusic("MainSpaceTheme");
                break;
            case 1:
                audioManager.Instance.PlayMusic("OribtronTheme");

                break;
            case 2:
                audioManager.Instance.PlayMusic("MainSpaceTheme");
                break;
            case 3:
                audioManager.Instance.PlayMusic("DrasilTheme");
                Debug.Log("Drasil Theme playing!");
                break;
            case 4:
                audioManager.Instance.PlayMusic("MainSpaceTheme");
                break;
            case 5:
                //audioManager.Instance.StopMusic("MainSpaceTheme");
                break;
            default:
                break;
        }
        Debug.Log("Current Index is " + currentIndex + ".Playing Respective music.");
    }

     bool checkIfSettingActive()
    {
        //temp fix-- will rewrite the pause code later
        if (settingsPanel.activeInHierarchy)
        {
            return true;
            //canPause = false;
            Debug.Log("Settings panel active");
           
        }
        else
        {
            return false;
           // canPause = true;
            Debug.Log("Settings panel inactive");
        }
    }
     int getCurrentIndex()
    {
        currentIndex = GetCurrentSceneIndex();
        Debug.Log("Current Index is "+ currentIndex);
        return currentIndex; 
    }

    //katie cutscene stuff
    //SceneManager.cs

    IEnumerator loadGoodEnd()
    {
        animScript.anim_.Play("fadeIn", 0, 0);
        yield return new WaitForSeconds(0.5f);
        currentIndex = 5;
        SceneManager.LoadScene("GOOD_CUTSCENE");

    }
    public void loadGoodCutscene()
    {
        StartCoroutine(loadGoodEnd());
        audioManager.Instance.StopMusic("MainSpaceTheme");
    }

    IEnumerator loadBadEnd()
    {
        animScript.anim_.Play("fadeIn", 0, 0);
        yield return new WaitForSeconds(0.5f);
        currentIndex = 5;
        audioManager.Instance.StopMusic("MainSpaceTheme");
        SceneManager.LoadScene("BAD_CUTSCENE");

    }
    public void loadBadCutscene()
    {
        StartCoroutine(loadBadEnd());
    }


}
