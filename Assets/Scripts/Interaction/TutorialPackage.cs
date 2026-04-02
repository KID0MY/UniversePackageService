using UnityEngine;

public class TutorialPackage : MonoBehaviour
{
    public void Start()
    {
        QuestManager _questManager = gameObject.GetComponent<PickUp>()._questManager;
        if (_questManager._tutorialFlagsCompleted > 3 && _questManager._dangerLevel < 2 && !_questManager.hasQuestObject)
        {
            gameObject.SetActive(false);
        }
    }
}
