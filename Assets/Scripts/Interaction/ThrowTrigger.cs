using UnityEngine;

public class ThrowTrigger : MonoBehaviour
{
    QuestManager _questManager;
    DialogueTrigger _npc;
    GameObject _package;

    void Start()
    {
        _npc = transform.parent.gameObject.GetComponent<DialogueTrigger>();
        _questManager = _npc._questManager;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_package == null)
        {
            _package = GameObject.Find("packagetwo_Updated(Clone)");
        }
        if (other.gameObject.Equals(_package))
        {
            if (_npc._wantsQuest && _questManager.hasQuestObject)
            {
                _npc.PackageDelivered(other.gameObject.GetComponent<PickUp>());
            }
        }
    }
}
