using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipTransition : Interactable
{
    public sceneManager_ sceneMan;
    public QuestManager questManager;
    public CharacterControl player;
    public bool returnShip;
    public GameObject _dialogueBox;

    public override void Awake()
    {
        if (_dialogueBox == null)
        {
            _dialogueBox = GameObject.Find("Dialogue");
        }
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        if (player == null)
        {
            player = FindAnyObjectByType<CharacterControl>();
        }
    }
    public override void OnFocus()
    {

    }

    public override void OnInteract()
    {
        if (questManager._tutorialFlagsCompleted == 3 && player.currentPickup == null)
        {
            _dialogueBox.GetComponent<Dialogue>().CreateDialogue("I told ya to load the package!");
        }
        else if (questManager._tutorialFlagsCompleted > 2)
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
                sceneMan.loadSpaceScene();
            }
            else
            {
                sceneMan.loadSpaceScene();
            }
        }
        else
        {
            _dialogueBox.GetComponent<Dialogue>().CreateDialogue("Leavin' so soon?");
            //player.BlowUpPlayer(this.gameObject);
        }
    }

    public override void OnLoseFocus()
    {

    }

    public override void CheckThrow()
    {
        
    }
}
