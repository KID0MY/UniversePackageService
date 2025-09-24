using NUnit.Framework;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject[] packagePrefabs;

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

        int packageType = Random.Range(0, 2);

        Vector3 spawnPoint = new Vector3(spawnPointX, spawnPointY, spawnPointZ);

        if(packageType == 0){Instantiate(packagePrefabs[1], spawnPoint, Quaternion.identity);}
        else{Instantiate(packagePrefabs[0], spawnPoint, Quaternion.identity);}
    }
}
