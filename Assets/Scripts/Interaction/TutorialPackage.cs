using UnityEngine;

public class TutorialPackage : MonoBehaviour
{
    public void Start()
    {
        print(gameObject.GetComponent<PickUp>()._questManager._tutorialFlagsCompleted);
        if (gameObject.GetComponent<PickUp>()._questManager._tutorialFlagsCompleted > 3)
        {
            gameObject.SetActive(false);
        }
    }
}
