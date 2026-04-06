using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public int _money;
    public bool _enableDebug;
    public List<string> _planetOneRecipientNames = new List<string>();
    public List<string> _planetTwoRecipientNames = new List<string>();
    public static QuestManager _instance;
    public Dialogue _dialoguer;
    public GameObject _questPrefab;
    public int _dangerLevel;
    public int _completedQuestNum = 0;
    public int _packagesForEnding = 4;
    public bool _bombPlanted = false;
    public List<Quest> _questList = new List<Quest>();
    public GameObject _questObjectPrefab;
    public CurrencyCounter _currencyCounter;
    public float _timePassed = 0f;
    public bool hasQuestObject;
    public int _tutorialFlagsCompleted;
    public DialogueFinal tutorialDialogue;

    void Awake() //Makes this node persist between scenes
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetupNameList() //I hate lists
    {
        if (_planetOneRecipientNames.Count == 0)
        {
            _planetOneRecipientNames.Add("Chrome");
            _planetOneRecipientNames.Add("Vanada");
            _planetOneRecipientNames.Add("Dent");
        }
        if (_planetTwoRecipientNames.Count == 0)
        {
            _planetTwoRecipientNames.Add("Orchid");
            _planetTwoRecipientNames.Add("Azalea");
            _planetTwoRecipientNames.Add("Weed");
        }
    }

    void Start()
    {
        SetupNameList();
        //autoendtutorial();
        //_enableDebug = false; Guys why are we force disabling the debug in the code you can set this from the inspector :sobbing_emoji:
    }

    public void AddActiveQuest(int planet_exclusion) //Creates a quest then adds in to _questList :3
    {
        GameObject child;
        child = Instantiate(_questPrefab) as GameObject;
        child.transform.parent = transform;
        child.GetComponent<Quest>()._dangerLevel = _dangerLevel;
        child.GetComponent<Quest>()._startTime = _timePassed;
        _questList.Add(child.GetComponent<Quest>());
        _questList[_questList.Count - 1].GenerateRandomQuest(planet_exclusion);
    }

    public void FinishActiveQuest(Quest quest)
    {
        for (int x = 0; x < _questList.Count; x++)
        {
            if (_questList[x] == quest)
            {
                int payout = quest._payAmount;
                int tips = 50;
                int time_taken = (int)(_timePassed - quest._startTime);
                if (quest._latenessLeeway > time_taken)
                {
                    time_taken = 0;
                }
                else
                {
                    time_taken -= quest._latenessLeeway;
                }
                tips -= time_taken;
                tips += Random.Range(0, 20); //Adds randomness to the tip value
                tips = (int)(tips * quest._health);
                if (tips < 0)
                {
                    tips = 0;
                }
                payout += tips;
                if (quest._health <= 0 || time_taken >= 50)
                {
                    payout = 0;
                }
                _money += payout;
                if (payout > 0)
                {
                    _currencyCounter.ShowGainedMoney(payout, _money);
                }
                _completedQuestNum++;
                Destroy(quest.gameObject);
                hasQuestObject = false;
                _questList.RemoveAt(x);
                break;
            }
        }
        if (_dangerLevel < 3)
        {
            _dangerLevel = _completedQuestNum;
        }
    }

    public bool IsMatchingRecipient(string recipient)
    {
        for (int x = 0; x < _questList.Count; x++)
        {
            if (_questList[x].GetComponent<Quest>()._recipient == recipient)
            {
                return true;
            }
        }
        return false;
    }

    public Quest GetQuestByRecipient(string recipient)
    {
        for (int x = 0; x < _questList.Count; x++)
        {
            if (_questList[x]._recipient == recipient)
            {
                return _questList[x];
            }
        }
        return null;
    }

    public bool CheckForFinale()
    {
        return _completedQuestNum == _packagesForEnding;
    }

    public void FinishTutorialFlag()
    {
        _tutorialFlagsCompleted++;
        if (_tutorialFlagsCompleted == 1) //First thing in the morning
        {
            _dialoguer.CreateDialogue(tutorialDialogue.Node[0]);
        }
        else if (_tutorialFlagsCompleted == 2) //Player gets to bossman's office
        {
            AddActiveQuest(-1);
            _dialoguer.CreateDialogue(tutorialDialogue.Node[1]);
        }
        else if (_tutorialFlagsCompleted == 3) //Player knows how to pick shit up
        {
            _dialoguer.CreateDialogue(tutorialDialogue.Node[2]);
        }
        else if (_tutorialFlagsCompleted == 4) //Player gets to space
        {
            _dialoguer.CreateDialogue(tutorialDialogue.Node[3]);
        }
        else if (_tutorialFlagsCompleted == 5) //Player gets to Orbitron
        {
            _dialoguer.CreateDialogue(tutorialDialogue.Node[4]);
        }
        else if (_tutorialFlagsCompleted == 6) //Player knows how to deliver package and finish the tutorial yippee
        {
            _dialoguer.CreateDialogue(tutorialDialogue.Node[5]);
            _dangerLevel = 1;
        }
    }

    void Update()
    {
        _timePassed += (Time.deltaTime);
        if (_enableDebug) //Debug commands, set this boolean to false to disable them
        {
            if (Input.GetKeyUp(KeyCode.Alpha0)) //0: warp to space
            {
                SceneManager.LoadScene("MAIN_SpaceScene");
            }
            if (Input.GetKeyUp(KeyCode.Alpha1)) //1: warp to planet 1
            {
                SceneManager.LoadScene("MAIN_Orbitron");
            }
            if (Input.GetKeyUp(KeyCode.Alpha2)) //2: warp to planet 2
            {
                SceneManager.LoadScene("MAIN_Drasil");
            }
            if (Input.GetKeyUp(KeyCode.Alpha3)) //3: warp to ups ship
            {
                SceneManager.LoadScene("MAIN_InsideShip");
            }
            if (Input.GetKeyUp(KeyCode.G)) //G: load a package into the ship, does not create a quest
            {
                hasQuestObject = true;
            }
            if (Input.GetKeyUp(KeyCode.R)) //R: reset player rotation
            {
                GameObject.Find("Player").transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (Input.GetKeyUp(KeyCode.F)) //F: increase character speed
            {
                GameObject.Find("Player").GetComponent<CharacterControl>().moveSpeed = 50f;
            }
            if (Input.GetKeyUp(KeyCode.P)) //P: create a new quest, automatically completing the last one if it existed, does not generate a package
            {
                if (_questList.Count > 0)
                {
                    FinishActiveQuest(_questList[0]);
                }
                AddActiveQuest(-1);
            }
            if (Input.GetKeyUp(KeyCode.T)) //T: cause one minute to pass
            {
                _timePassed += 60;
            }
            if (Input.GetKeyUp(KeyCode.M)) //M: prints the current amount of money
            {
                print(_money);
            }
            if (Input.GetKeyUp(KeyCode.K)) //K: spawns a package object on your head
            {
                GameObject _questObject = Instantiate(_questObjectPrefab, GameObject.Find("Player").transform.position + Vector3.up, Quaternion.identity);
                _questObject.GetComponent<PickUp>().OnInteract();
            }
            if (Input.GetKeyUp(KeyCode.L)) //L: automatically end the tutorial
            {
                autoendtutorial();
            }
            if (Input.GetKeyDown(KeyCode.Y)) //Y: boss will offer the ending package
            {
                _completedQuestNum = _packagesForEnding;
                _dangerLevel = 3;
            }
        }
    }

    void autoendtutorial() //Just a plaaceholder function for funsies
    {
        _dangerLevel = 1;
        _completedQuestNum = 1;
        _tutorialFlagsCompleted = 1000;
    }
}
