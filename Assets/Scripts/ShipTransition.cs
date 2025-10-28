using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipTransition : Interactable
{
    public sceneManager_ sceneMan;
    public bool returnShip;
    public override void OnFocus()
    {

    }

    public override void OnInteract()
    {
        if (!returnShip)
        {
            sceneMan.loadNextScene();
        }
        else
        {
            sceneMan.returnToMain();
        }
    }

    public override void OnLoseFocus()
    {

    }
}
