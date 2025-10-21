using System.Linq;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject[] rockPrefabs;

    public int maxRange;
    public int minRange;

    public int maxCount = 100;
    int Count;



    // Update is called once per frame
    void Update()
    {
        if (Count < maxCount)
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

        if (spawnCheck.Length == 1)
        {
            Instantiate(rockPrefabs[rockType], spawnPoint, Quaternion.identity);
        }
        
    }
}
