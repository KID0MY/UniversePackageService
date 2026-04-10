using UnityEngine;

public class CredManBadEnd : MonoBehaviour
{
    public GameObject creditsUI;
    public QuestManager questManager;
    float waitTime = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }

    // Update is called once per frame
    void Update()
    {
        waitTime += Time.deltaTime;
        if (waitTime > 13)
        {
            creditsUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
    }

    public void Reset()
    {
        questManager.Reset();
    }
}
