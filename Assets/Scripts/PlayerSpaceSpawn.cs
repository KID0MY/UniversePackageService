using UnityEngine;

public class PlayerSpaceSpawn : MonoBehaviour
{
    public GameObject[] exitNodes;

    void Start()
    {
        // This finds all active objects tagged "ShipExitNode"
        exitNodes = GameObject.FindGameObjectsWithTag("ShipExitNode");

        Debug.Log($"Found {exitNodes.Length} exit nodes.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
