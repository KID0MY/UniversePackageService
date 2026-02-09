using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public int _money;
    public bool _enableDebug;
    public List<string> _planetOneRecipientNames = new List<string>();
    public List<string> _planetTwoRecipientNames = new List<string>();
    public static QuestManager _instance;
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
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void SetupNameList() //I hate lists
    {
        _planetOneRecipientNames.Add("Chrome");
        _planetOneRecipientNames.Add("Vanada");
        _planetTwoRecipientNames.Add("Orchid");
        _planetTwoRecipientNames.Add("Azalea");
    }

    void Start()
    {
        SetupNameList();
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
                int time_taken = (int)(quest._startTime - _timePassed);
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
        if (_tutorialFlagsCompleted == 1) //Player knows how to move
        {
            
        }
        else if (_tutorialFlagsCompleted == 2) //Player knows how to jump
        {
            GameObject _questObject = Instantiate(_questObjectPrefab, new Vector3(0, 10, 0), Quaternion.identity);
            _questObject.gameObject.layer = 6;
            _questObject.GetComponent<Rigidbody>().isKinematic = false;
            _questObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (_tutorialFlagsCompleted == 3) //Player knows how to pick shit up
        {
            
        }
        else if (_tutorialFlagsCompleted == 4) //Player knows how to put shit down
        {
            AddActiveQuest(-1);
        }
        else if (_tutorialFlagsCompleted == 5) //Player knows how to move IN SPACE
        {
            
        }
        else if (_tutorialFlagsCompleted == 6) //Player knows how to do a barrel roll
        {
            
        }
        else if (_tutorialFlagsCompleted == 7) //Player knows how to boost
        {

        }
        else if (_tutorialFlagsCompleted == 8) //Player knows how to deliver package and finish the tutorial yippee
        {
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
                SceneManager.LoadScene("MAIN_GameScene");
            }
            if (Input.GetKeyUp(KeyCode.Alpha1)) //1: warp to planet 1
            {
                SceneManager.LoadScene("MAIN_Greybox1");
            }
            if (Input.GetKeyUp(KeyCode.Alpha2)) //2: warp to planet 2
            {
                SceneManager.LoadScene("MAIN_Greybox2");
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
                _dangerLevel = 1;
                _tutorialFlagsCompleted = 1000;
            }
        }
    }
}
