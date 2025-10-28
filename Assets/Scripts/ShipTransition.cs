using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipTransition : Interactable
{
    public sceneManager_ sceneMan;
    public override void OnFocus()
    {

    }

    public override void OnInteract()
    {
        sceneMan.loadNextScene();
    }

    public override void OnLoseFocus()
    {

    }
}
