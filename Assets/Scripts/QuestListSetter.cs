using TMPro;
using UnityEngine;

public class QuestListSetter : MonoBehaviour
{
    public GameObject _slot1;
    public QuestManager _manager;

    void Start()
    {
        _manager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }

    void Update()
    {
        
    }

    public void SetQuests()
    {
        if (_manager == null)
        {
            _manager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        }
        GameObject _name = _slot1.transform.GetChild(0).gameObject;
        GameObject _description = _slot1.transform.GetChild(1).gameObject;
        GameObject _status = _slot1.transform.GetChild(2).gameObject;
        _name.GetComponent<TMPro.TextMeshProUGUI>().text = _manager._questList[0].GetComponent<Quest>()._questName;
        _description.GetComponent<TMPro.TextMeshProUGUI>().text = "Instructions: " + _manager._questList[0].GetComponent<Quest>()._description;
        _status.GetComponent<TMPro.TextMeshProUGUI>().text = "Status: Secure";
    }
}
