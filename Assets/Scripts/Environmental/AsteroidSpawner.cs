using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class AsteroidSpawner : MonoBehaviour
{
    public GameObject[] rockPrefabs;

    public int maxRange;
    public int minRange;
    public Transform _orbitronLocation;
    public Transform _drasilLocation;
    public Transform _playerLocation;
    public Transform _shipLocation;
    public float _planetLeeway;
    public float _playerLeeway;
    public float _shipLeeway;
    float _checkTimer = 0f;
    float _checkTime = 1f;
    public int maxCount = 100;
    int Count;
    public List<Asteroid> _asteroidObjects = new List<Asteroid>();

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
        int spawnPointY = Random.Range(minRange, maxRange) + 60;
        int spawnPointZ = Random.Range(minRange, maxRange) + 786;

        int rockType = Random.Range(0, rockPrefabs.Length);

        SphereCollider col = rockPrefabs[rockType].GetComponent<SphereCollider>();
        //Collider[] spawnCheck = Physics.OverlapSphere(col.center, col.radius);

        Vector3 spawnPoint = new Vector3(spawnPointX, spawnPointY, spawnPointZ);
        GameObject _asteroid = Instantiate(rockPrefabs[rockType], spawnPoint, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
        _asteroid.transform.localScale = _asteroid.transform.localScale * Random.Range(20, 60);
        if (Vector3.Distance(_asteroid.transform.position, _orbitronLocation.position) < _planetLeeway)
        {
            Destroy(_asteroid);
        }
        else if (Vector3.Distance(_asteroid.transform.position, _drasilLocation.position) < _planetLeeway)
        {
            Destroy(_asteroid);
        }
        else if (Vector3.Distance(_asteroid.transform.position, _shipLocation.position) < _shipLeeway)
        {
            Destroy(_asteroid);
        }
        else if (Vector3.Distance(_asteroid.transform.position, _playerLocation.position) < _playerLeeway)
        {
            Destroy(_asteroid);
        }
        else
        {
            _asteroidObjects.Add(_asteroid.GetComponent<Asteroid>());
        }
        //if (spawnCheck.Length == 1)
        //{
        //    Debug.Log("pip");
        //    Instantiate(rockPrefabs[rockType], spawnPoint, Quaternion.identity);
        //}
    }

    public void Update()
    {
        _checkTimer += Time.deltaTime;
        for (int i = 0; i  < _asteroidObjects.Count; i++)
        {
            if (_checkTimer > _checkTime)
            {
                _checkTimer = 0;
                //if (Vector3.Distance(_asteroidObjects[i]._player.transform.position, _asteroidObjects[i].transform.position) < 200)
                //{
                //    _asteroidObjects[i].enabled = true;
                //}
                //else
                //{
                //    _asteroidObjects[i].enabled = false;
                //}
                _asteroidObjects[i].CheckCollisionEnable();
            }
            _asteroidObjects[i].CheckCollisionEnable();
            if (Vector3.Distance(_asteroidObjects[i].transform.position, _orbitronLocation.position) < _planetLeeway)
            {
                _asteroidObjects[i].InvertDirection();
            }
            else if (Vector3.Distance(_asteroidObjects[i].transform.position, _drasilLocation.position) < _planetLeeway)
            {
                _asteroidObjects[i].InvertDirection();
            }
            else if (Vector3.Distance(_asteroidObjects[i].transform.position, _shipLocation.position) < _shipLeeway)
            {
                _asteroidObjects[i].InvertDirection();
            }
            else if (Vector3.Distance(_asteroidObjects[i].transform.position, _playerLocation.position) > maxRange)
            {
                _asteroidObjects[i].TargetPlayer();
            }
        }
    }
}
