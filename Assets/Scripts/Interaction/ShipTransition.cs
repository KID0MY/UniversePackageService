using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipTransition : Interactable
{
    public sceneManager_ sceneMan;
    public QuestManager questManager;
    public CharacterControl player;
    public bool returnShip;
    public GameObject _dialogueBox;
    private void Start()
    {
        _dialogueBox = GameObject.Find("Dialogue");
        questManager = FindAnyObjectByType<QuestManager>();
        player = FindAnyObjectByType<CharacterControl>();
    }
    public override void OnFocus()
    {

    }

    public override void OnInteract()
    {
        if (questManager._tutorialFlagsCompleted > 3)
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
        else
        {
            _dialogueBox.GetComponent<Dialogue>().CreateDialogue("There's a time and place for everything, but not now.");
            //player.BlowUpPlayer(this.gameObject);
        }
    }

    public override void OnLoseFocus()
    {

    }
}
