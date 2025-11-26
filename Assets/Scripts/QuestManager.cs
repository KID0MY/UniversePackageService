using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager _instance;
    public GameObject _questPrefab;
    public int _dangerLevel = 0;
    public List<GameObject> _questList = new List<GameObject>();
    public float _timePassed = 0f;

    void Awake() //Makes this node persist between scenes
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start() //Basic implementation to just shove a quest. None of the code in start will be in the final game.
    {
        AddActiveQuest();
    }

    void AddActiveQuest() //Creates a quest then adds in to _questList :3
    {
        GameObject child;
        child = Instantiate(_questPrefab) as GameObject;
        child.transform.parent = transform;
        child.GetComponent<Quest>()._dangerLevel = _dangerLevel;
        child.GetComponent<Quest>()._startTime = _timePassed;
        _questList.Add(child);
        _questList[_questList.Count - 1].GetComponent<Quest>().GenerateRandomQuest();
    }

    void Update()
    {
<<<<<<< HEAD
        //_timePassed += (Time.deltaTime * 1000000);
        //print(_timePassed);
=======
        _timePassed += (Time.deltaTime);
>>>>>>> parent of 1ea7142 (Merge pull request #37 from KID0MY/main)
    }
}
