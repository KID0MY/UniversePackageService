using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> packagePrefabs = new List<GameObject>();

    public int maxRange;
    public int minRange;

    public int maxCount = 100;
    int Count;

    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

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

        int packageType = Random.Range(0, 2);

        Vector3 spawnPoint = new Vector3(spawnPointX, spawnPointY, spawnPointZ);

        Instantiate(packagePrefabs[packageType], spawnPoint, Quaternion.identity);
    }
}
