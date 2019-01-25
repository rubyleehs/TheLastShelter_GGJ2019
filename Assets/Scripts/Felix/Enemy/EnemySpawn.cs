using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    PoolManager poolManager;
    public GameObject[] objPrefabs;
    public Transform[] spawnPoint;

    public float spawnCooldown; //the cooldown should vary upon difficuly (later waves has lower cooldown)
    public static float spawnTimer; //used to count down to spawn

    void Start()
    {
        poolManager = GetComponent<PoolManager>();
        //objPrefabs.Length indicate the pool manager to spawn same X amount for each objPrefabs dragged in Unity's inspector
        for (int x = 0; x < objPrefabs.Length; x++)
        {
            //this line do the game object instantiations, which objPrefab and how many to spawn
            poolManager.CreatePool(objPrefabs[x], 100);
        }

        //Initialize spawn cooldown
        spawnCooldown = 3f;
    }

    void Update()
    {
        if (spawnTimer > 0)
        {
            spawnTimer -= GameManager.deltaTime;
        }
        else //timer is finished! spawn enemy
        {
            spawnTimer = spawnCooldown;
            spawnEnemy();
        }
    }

    void spawnEnemy()
    {
        int prefabToSpawnIndex = Random.Range(0, objPrefabs.Length);
        int posToSpawn = Random.Range(0, spawnPoint.Length);

        poolManager.ReuseObject(objPrefabs[prefabToSpawnIndex], spawnPoint[posToSpawn].position, spawnPoint[posToSpawn].rotation);
    }
}
