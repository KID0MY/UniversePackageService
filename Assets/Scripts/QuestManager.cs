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
    public List<GameObject> _questList = new List<GameObject>();
    public GameObject _questObjectPrefab;
    public CurrencyCounter _currencyCounter;
    public float _timePassed = 0f;
    public bool hasQuestObject;
    public int _tutorialFlagsCompleted;

    void Awake() //Makes this node persist between scenes
    {
        if (FindFirstObjectByType<QuestManager>().Equals(this))
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
        _planetOneRecipientNames.Clear();
        _planetTwoRecipientNames.Clear();
        _planetOneRecipientNames.Add("Chrome");
        _planetOneRecipientNames.Add("Vanada");
        _planetTwoRecipientNames.Add("Orchid");
        _planetTwoRecipientNames.Add("Azalea");
    }

    void Start()
    {
        SetupNameList();
        //autoendtutorial();
        _enableDebug = false;
    }

    public void AddActiveQuest(int planet_exclusion) //Creates a quest then adds in to _questList :3
    {
        GameObject child;
        child = Instantiate(_questPrefab) as GameObject;
        child.transform.parent = transform;
        child.GetComponent<Quest>()._dangerLevel = _dangerLevel;
        child.GetComponent<Quest>()._startTime = _timePassed;
        _questList.Add(child);
        _questList[_questList.Count - 1].GetComponent<Quest>().GenerateRandomQuest(planet_exclusion);
    }

    public void FinishActiveQuest(Quest quest)
    {
        for (int x = 0; x < _questList.Count; x++)
        {
            if (_questList[x].GetComponent<Quest>() == quest)
            {
                int payout = quest._payAmount; //Nothing actually happens with this value.
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
                Destroy(quest.gameObject);
                hasQuestObject = false;
                _questList.RemoveAt(x);
                break;
            }
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
            if (_questList[x].GetComponent<Quest>()._recipient == recipient)
            {
                return _questList[x].GetComponent<Quest>();
            }
        }
        return null;
    }

    public void FinishTutorialFlag()
    {
        _tutorialFlagsCompleted++;
        if (_tutorialFlagsCompleted == 1) //First thing in the morning
        {
            _dialoguer.CreateDialogue("G’doy matey, an’ welcome t’yer first day at Universal Parcel Service.");
            _dialoguer.CreateDialogue("Ye’ll be expected t’deliver packages t’our customers, an’ yer gonna do it well. Got it?");
            _dialoguer.CreateDialogue("So, I’m in charge o’ onboarding an’ all that. First though, I’m gonna need ya t’come t’my office, ‘kay?");
        }
        else if (_tutorialFlagsCompleted == 2) //Player gets to bossman's office
        {
            AddActiveQuest(-1);
            _dialoguer.CreateDialogue("‘Ey mate, good t’finally see ya in person.");
            _dialoguer.CreateDialogue("Now, ‘ere’s a package we need delivered soon, an’ all our other employees are kinda busy right now, so this is yer problem.");
            _dialoguer.CreateDialogue("Go ahead an’ grab it with E.");
        }
        else if (_tutorialFlagsCompleted == 3) //Player knows how to pick shit up
        {
            _dialoguer.CreateDialogue("Cool, be careful with the package by the way. Wouldn’t wanna mess up our reputation any more than it already has.");
            _dialoguer.CreateDialogue("Next I need ye t’load this package onto yer ship. Pick up the package an’ press E t’get in.");
            _dialoguer.CreateDialogue("An’ don’t ye dare forget t’put it in the ship. If y’did yer gonna have t’report it to the recipient. Got it?");
        }
        else if (_tutorialFlagsCompleted == 4) //Player gets to space
        {
            _dialoguer.CreateDialogue("Congrats on yer first take-off. Pray it won’t be yer last.");
            _dialoguer.CreateDialogue("Fer a crash course on how not t’crash, move around usin’ our patented UPS thruster technology.");
            _dialoguer.CreateDialogue("Oh, and press TAB to see the ship’s log, it tells ya everything ya oughta know about yer deliveries.");
            _dialoguer.CreateDialogue("Right, that package I gave ya needs to go to Orbitron. That’s the purple one in case ya live under an asteroid.");
            _dialoguer.CreateDialogue("Just fly over there, the ships fancy parking systems oughta have the rest covered.");
        }
        else if (_tutorialFlagsCompleted == 5) //Player gets to Orbitron
        {
            _dialoguer.CreateDialogue("Talk with the fellow marked on yer minimap t’leave the package with ‘em. They’ll pay ye fer yer service!");
        }
        else if (_tutorialFlagsCompleted == 6) //Player knows how to deliver package and finish the tutorial yippee
        {
            _dialoguer.CreateDialogue("Congrats on yer first official delivery! I’m all outta lessons for ye, so now yer on yer own.");
            _dialoguer.CreateDialogue("If ye keep performin’ this well, we higher-ups may have somethin’ special for ye to deliver…");
            _dialoguer.CreateDialogue("Go around the planet t’find out who else needs t’deliver somethin’. You’ll see ‘em marked on your quest log when ye accept.");
            _dialoguer.CreateDialogue("Remember, always do the right thing fer the company. Ye’ll be set up fer success that way.");
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
                    FinishActiveQuest(_questList[0].GetComponent<Quest>());
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
        }
    }

    void autoendtutorial() //Just a plaaceholder function for funsies
    {
        _dangerLevel = 1;
        _tutorialFlagsCompleted = 1000;
    }
}
