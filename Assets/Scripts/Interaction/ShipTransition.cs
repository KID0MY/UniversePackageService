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
        if (GameObject.Find("Player").GetComponent<CharacterControl>().isHolding)
        {
            GameObject.Find("QuestManager").GetComponent<QuestManager>().hasQuestObject = true;
        }
        else
        {
            GameObject.Find("QuestManager").GetComponent<QuestManager>().hasQuestObject = false;
        }
        if (!returnShip)
        {
            sceneMan.loadNextScene();
        }
        else
        {
            sceneMan.loadLastScene();
        }
    }

    public override void OnLoseFocus()
    {

    }
}
