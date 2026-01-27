using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public QuestManager _questManager;    

    public GameObject visualCue;
        
    public GameObject dialogueBox;

    public TMPro.TextMeshProUGUI _dialogueText;

    public GameObject questObjectPrefab;

    public string name;

    public int _planetNum;

    public bool isQuestGiver;

    public bool isQuestReceiver;

    public int _recieverNameId;

    private GameObject questObject;

    private bool playerInRange;

    private bool _hasQuest;

    private bool _wantsQuest;
   
    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Start()
    {
        _hasQuest = true;
        _wantsQuest = false;
        _questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        _dialogueText = dialogueBox.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
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
            if (Input.GetKeyDown(KeyCode.E))
            {
                _dialogueText.text = "Good morning.";
                dialogueBox.SetActive(true);
                if (_hasQuest && isQuestGiver && _questManager._questList.Count == 0)
                {
                    GivePackage();
                    Quest _questObject = _questManager._questList[0].GetComponent<Quest>();
                    _dialogueText.text = "Take this and bring it to " + _questObject._recipient + " on " + _questObject._destination + "\nPress Tab to view details.";
                }
                if (_wantsQuest && _questManager.hasQuestObject && GameObject.Find("Player").GetComponent<CharacterControl>().isHolding)
                {
                    GameObject.Find("Player").GetComponent<CharacterControl>().currentPickup.GetComponent<PickUp>().KILLYOURSELF();
                    Quest _questObject = _questManager._questList[0].GetComponent<Quest>();
                    if (_questObject._health <= 0 && _questObject.GetTimeTaken() >= (50 + _questObject._latenessLeeway))
                    {
                        _dialogueText.text = "Not only did you take forever, but everything in here is gone. I'm not paying for this.";
                    }
                    else if (_questObject._health <= 0)
                    {
                        _dialogueText.text = "All of the contents are destroyed! I'm not paying you for this.";
                    }
                    else if (_questObject.GetTimeTaken() >= (50 + _questObject._latenessLeeway))
                    {
                        _dialogueText.text = "You took too long! I'm not paying you for this.";
                    }
                    else
                    {
                        _dialogueText.text = "Thank you!";
                    }
                    _questManager.FinishActiveQuest(_questManager.GetQuestByRecipient(name));
                    _wantsQuest = false;
                }
                else if (_wantsQuest && GameObject.Find("packagetwo_Updated(Clone)") == null)
                {
                    _dialogueText.text = "What do you mean you \"lost\" my package???";
                    Destroy(_questManager._questList[0]);
                    _questManager.hasQuestObject = false;
                    _questManager._questList.RemoveAt(0);
                    _wantsQuest = false;
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
            other.transform.parent.GetComponent<CharacterControl>()._isDropDisabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
            other.transform.parent.GetComponent<CharacterControl>()._isDropDisabled = false;
            dialogueBox.SetActive(false);
        }
    }
}
