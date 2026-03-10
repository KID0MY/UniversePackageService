using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public bool _enableDebug;
    public List<string> _planetOneRecipientNames = new List<string>();
    public List<string> _planetTwoRecipientNames = new List<string>();
    public static QuestManager _instance;
    public GameObject _questPrefab;
    public int _dangerLevel = 0;
    public List<GameObject> _questList = new List<GameObject>();
    public float _timePassed = 0f;
    public bool hasQuestObject;

    void Awake() //Makes this node persist between scenes
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        SetupNameList();
    }

    private void SetupNameList() //I hate lists
    {
        _planetOneRecipientNames.Add("Metal Gleepglorp");
        _planetTwoRecipientNames.Add("Wild Gleepglorp");
    }

    void Start()
    {
        
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
                int time_taken = 50 - ((int)_timePassed - (int)quest._startTime);
                if (time_taken < quest._latenessLeeway) //Subtracts the time taken to deliver by the quests given lateness leeway
                {
                    time_taken = 0;
                }
                else
                {
                    time_taken -= quest._latenessLeeway;
                }
                int tips = time_taken;
                tips += Random.Range(0, 20); //Adds randomness to the tip value
                if (tips < 0)
                {
                    tips = 0;
                }
                payout += tips;
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
        }
    }
}
