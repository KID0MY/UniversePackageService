using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager _instance;
    public GameObject _questPrefab;
    public List<GameObject> _questList = new List<GameObject>();

    void Awake() //Makes this node persist between scenes
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start() //Basic implementation to just shove 4 quests and print them all to console. None of the code in start will be in the final game.
    {
        AddActiveQuest();
        AddActiveQuest();
        AddActiveQuest();
        AddActiveQuest();
        for (int i = 0; i <= 3; i++)
        {
            print(_questList[i].GetComponent<Quest>()._questName + " to " + _questList[i].GetComponent<Quest>()._destination + ": " + _questList[i].GetComponent<Quest>()._description + " For $" + _questList[i].GetComponent<Quest>()._payAmount);
        }
    }

    void AddActiveQuest() //Creates a quest then adds in to _questList :3
    {
        GameObject child;
        child = Instantiate(_questPrefab) as GameObject;
        child.transform.parent = transform;
        _questList.Add(child);
        _questList[_questList.Count - 1].GetComponent<Quest>().GenerateRandomQuest();
    }

    void Update()
    {
        
    }
}
