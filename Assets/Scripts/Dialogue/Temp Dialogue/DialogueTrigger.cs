using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public QuestManager _questManager;    

    public GameObject visualCue;
        
    public GameObject dialogueBox;

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
                dialogueBox.SetActive(true);
                if (_hasQuest && isQuestGiver && _questManager._questList.Count == 0)
                {
                    GivePackage();
                }
                if (_wantsQuest && _questManager.hasQuestObject && GameObject.Find("Player").GetComponent<CharacterControl>().isHolding)
                {
                    GameObject.Find("Player").GetComponent<CharacterControl>().currentPickup.GetComponent<PickUp>().KILLYOURSELF();
                    _questManager.FinishActiveQuest(_questManager.GetQuestByRecipient(name));
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
            dialogueBox.SetActive(false);
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
        }
    }
}
