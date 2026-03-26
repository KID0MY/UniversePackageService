using UnityEngine;
using System.Collections.Generic;

public class DialogueTrigger : MonoBehaviour
{
    public QuestManager _questManager;    

    public GameObject visualCue;
        
    public GameObject dialogueBox;

    public Dialogue _dialogueScr;

    public CharacterControl _player;

    public GameObject questObjectPrefab;

    public string name;

    public int _planetNum;

    public bool isQuestGiver;

    public bool isQuestReceiver;

    public int _recieverNameId;

    private GameObject questObject;

    private bool playerInRange;

    private bool _hasQuest;

    public bool _wantsQuest;
   
    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
        _questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }

    private void Start()
    {
        _hasQuest = true;
        _wantsQuest = false;
        if (_player == null)
        {
            _player = GameObject.Find("Player").GetComponent<CharacterControl>();
        }
        if (_dialogueScr == null)
        {
            _dialogueScr = dialogueBox.GetComponent<Dialogue>();
        }
        if (isQuestReceiver)
        {
            if (_planetNum == 0)
            {
                name = _questManager._planetOneRecipientNames[_recieverNameId];
            }
            else
            {
                name = _questManager._planetTwoRecipientNames[_recieverNameId];
            }
            if (_questManager.IsMatchingRecipient(name))
            {
                _wantsQuest = true;
            }
        }
    }

    private void Update()
        {
        if (playerInRange)
        {
            if (isQuestGiver && _hasQuest && _questManager._questList.Count == 0)
            {
                visualCue.SetActive(true);
            }
            else if (_wantsQuest)
            {
                visualCue.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.E) && !_player._cutsceneMovementLock && _questManager._tutorialFlagsCompleted >= 5)
            {
                if (_hasQuest && isQuestGiver && _questManager._questList.Count == 0)
                {
                    GivePackage();
                    Quest _questObject = _questManager._questList[0].GetComponent<Quest>();
                    _dialogueScr.CreateDialogue("Take this and bring it to " + _questObject._recipient + " on " + _questObject._destination + "\nPress Tab to view details.");
                }
                else if (_wantsQuest && _questManager.hasQuestObject && GameObject.Find("Player").GetComponent<CharacterControl>().isHolding)
                {
                    PackageDelivered(_player.currentPickup.GetComponent<PickUp>());
                }
                else if (_wantsQuest && GameObject.Find("packagetwo_Updated(Clone)") == null)
                {
                    _dialogueScr.CreateDialogue("What do you mean you \"lost\" my package???");
                    Destroy(_questManager._questList[0]);
                    _questManager.hasQuestObject = false;
                    _questManager._questList.RemoveAt(0);
                    _wantsQuest = false;
                }
                else
                {
                    _dialogueScr.CreateDialogue("Good morning.");
                }
            }
        }
        else
        {
            if (isQuestGiver)
            {
                visualCue.SetActive(false);
            }
            else if (_wantsQuest)
            {
                visualCue.SetActive(true);
            }
        }
    }

    public void PackageDelivered(PickUp package)
    {
        package.KILLYOURSELF();
        Quest _questObject = _questManager._questList[0].GetComponent<Quest>();
        if (_questObject._health <= 0 && _questObject.GetTimeTaken() >= (50 + _questObject._latenessLeeway))
        {
            _dialogueScr.CreateDialogue("Not only did you take forever, but everything in here is gone. I'm not paying for this.");
        }
        else if (_questObject._health <= 0)
        {
            _dialogueScr.CreateDialogue("All of the contents are destroyed! I'm not paying you for this.");
        }
        else if (_questObject.GetTimeTaken() >= (50 + _questObject._latenessLeeway))
        {
            _dialogueScr.CreateDialogue("You took too long! I'm not paying you for this.");
        }
        else
        {
            _dialogueScr.CreateDialogue("Thank you!");
        }
        if (_questManager._tutorialFlagsCompleted <= 5)
        {
            _questManager.FinishTutorialFlag();
        }
        _questManager.FinishActiveQuest(_questManager.GetQuestByRecipient(name));
        _wantsQuest = false;
    }

    public void GivePackage()
    {
        print("god giveth");
        _hasQuest = false;
        questObject = Instantiate(questObjectPrefab, this.transform.position + Vector3.right, Quaternion.identity);
        questObject.GetComponent<PickUp>().OnInteract();
        _questManager.AddActiveQuest(_planetNum);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
            _player._isDropDisabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
            _player._isDropDisabled = false;
        }
    }
}
