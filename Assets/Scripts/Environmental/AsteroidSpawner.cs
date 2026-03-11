using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject[] rockPrefabs;

    public int maxRange;
    public int minRange;

    public int maxCount = 100;
    int Count;



    // Update is called once per frame
    void Awake()
    {
        while(Count < maxCount)
        {
            SpawnPackage();
            Count++;
        }
    }

    public void SpawnPackage()
    {
        int spawnPointX = Random.Range(minRange, maxRange);
        int spawnPointY = Random.Range(minRange, maxRange);
        int spawnPointZ = Random.Range(minRange, maxRange);

        int rockType = Random.Range(0, rockPrefabs.Length);

        SphereCollider col = rockPrefabs[rockType].GetComponent<SphereCollider>();
        Collider[] spawnCheck = Physics.OverlapSphere(col.center, col.radius);

        Vector3 spawnPoint = new Vector3(spawnPointX, spawnPointY, spawnPointZ);
        Instantiate(rockPrefabs[rockType], spawnPoint, Quaternion.identity);
        if (spawnCheck.Length == 1)
        {
            Debug.Log("pip");
            Instantiate(rockPrefabs[rockType], spawnPoint, Quaternion.identity);
        }
        
    }
}
