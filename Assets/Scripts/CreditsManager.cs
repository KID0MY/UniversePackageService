using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    public GameObject creditsUI;
    float waitTime = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        waitTime += Time.deltaTime;
        if (waitTime > 8)
        {
            creditsUI.SetActive(true);
            

        }
    }
}
