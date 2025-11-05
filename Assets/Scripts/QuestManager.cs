using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager _instance;
    public Quest _questPrefab;
    public List<GameObject> _questList = new List<GameObject>();

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        AddActiveQuest();
    }

    void AddActiveQuest()
    {
        Instantiate(_questPrefab);
        GameObject _newQuest = gameObject.transform.GetChild(gameObject.transform.childCount - 1).gameObject;
        _questList.Add(_newQuest);
        _newQuest.GetComponent<Quest>().GenerateRandomQuest();
    }

    void Update()
    {
        
    }
}
