using UnityEngine;

public class PlayerSpaceSpawn : MonoBehaviour
{
    public GameObject[] exitNodes;
    public QuestManager questManager;
    public GameObject playerShip;

    void Awake()
    {
        // This finds all active objects tagged "ShipExitNode"
        exitNodes = GameObject.FindGameObjectsWithTag("ShipExitNode");

        Debug.Log($"Found {exitNodes.Length} exit nodes.");

        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();

        if (questManager.lastScene.Contains("Orbitron"))
        {
            playerShip.transform.position = exitNodes[2].transform.position;
            playerShip.transform.rotation = exitNodes[2].transform.rotation;
        }
        else if (questManager.lastScene.Contains("Drasil"))
        {
            playerShip.transform.position = exitNodes[1].transform.position;
            playerShip.transform.rotation = exitNodes[1].transform.rotation;
        }
        else if (questManager.lastScene.Contains("Inside"))
        {
            playerShip.transform.position = exitNodes[0].transform.position;
            playerShip.transform.rotation = exitNodes[0].transform.rotation;
        }
    }
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
