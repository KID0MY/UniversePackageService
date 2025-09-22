
using UnityEngine;
using UnityEngine.SceneManagement;


public class sceneManager_ : MonoBehaviour
{
    public int currentScene;
    public int newScene;
    public string sceneName;

    public void returnToMain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Debug.Log("Current Build index is " + SceneManager.GetActiveScene().buildIndex);
    }

    public void loadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Current Build index is " + SceneManager.GetActiveScene().buildIndex);

    }
    public void exitGame()
    {
        Debug.Log("Exiting!");
    }

    public void resetScene()
    {
        Debug.Log("ResettingScene");
        //for starting the game over again
    }
}
