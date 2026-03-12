using TMPro;
using UnityEngine;

public class QuestListSetter : MonoBehaviour
{
    public GameObject _slot1;
    public QuestManager _manager;
    public CharacterControl _player;

    void Start()
    {
        _manager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        if (_player == null)
        {
            _player = GameObject.Find("Player").GetComponent<CharacterControl>();
        }
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
        if (_manager._questList.Count > 0)
        {
            _name.GetComponent<TMPro.TextMeshProUGUI>().text = _manager._questList[0].GetComponent<Quest>()._questName;
            _description.GetComponent<TMPro.TextMeshProUGUI>().text = "Instructions: " + _manager._questList[0].GetComponent<Quest>()._description;
            if (_manager.hasQuestObject)
            {
                _status.GetComponent<TMPro.TextMeshProUGUI>().text = "Status: Secure";
                if (_manager._questList[0].GetComponent<Quest>()._health < 0f)
                {
                    _status.GetComponent<TMPro.TextMeshProUGUI>().text = "Status: Destroyed";
                }
                else if (_manager._questList[0].GetComponent<Quest>()._health < 0.5f)
                {
                    _status.GetComponent<TMPro.TextMeshProUGUI>().text = "Status: Damaged";
                }
            }
            else if (_player == null)
            {
                _status.GetComponent<TMPro.TextMeshProUGUI>().text = "Status: Lost";
            }
            else if (_player.currentPickup != null)
            {
                _status.GetComponent<TMPro.TextMeshProUGUI>().text = "Status: Held";
            }
            else
            {
                _status.GetComponent<TMPro.TextMeshProUGUI>().text = "Status: Lost";
            }
        }
        else
        {
            _name.GetComponent<TMPro.TextMeshProUGUI>().text = "Currently no quest";
            _description.GetComponent<TMPro.TextMeshProUGUI>().text = "Find someone who needs a parcel delivered!";
            _status.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }
    }
}
