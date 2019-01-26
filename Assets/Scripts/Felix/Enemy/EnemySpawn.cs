using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    PoolManager poolManager;

    public GameObject[] objPrefabs;
    enum enemyType { normalEnemy, fastEnemy, bigEnemy }

    public Transform[] spawnPoint;

    public static float spawnTimer; //used to count down to spawn

    void Start()
    {
        poolManager = GetComponent<PoolManager>();
        //objPrefabs.Length indicate the pool manager to spawn same X amount for each objPrefabs dragged in Unity's inspector
        for (int x = 0; x < objPrefabs.Length; x++)
        {
            //this line do the game object instantiations, which objPrefab and how many to spawn
            poolManager.CreatePool(objPrefabs[x], 50);
        }

        spawnTimer = WaveManager.currentWave.spawnSpeed;
    }

    void Update()
    {       
        if (spawnTimer > 0)
        {
            spawnTimer -= GameManager.deltaTime;
        }
        else //timer is finished! spawn enemy
        {
            spawnTimer = WaveManager.currentWave.spawnSpeed;
            spawnEnemy();
        }
    }

    void spawnEnemy()
    {
        //determine which enemy should be spawn based on currentWave left enemies
        //only spawn enemies when there is leftover enemies in that particular wave to spawn
        //add in enemy's index if they are available to spawn
        List<int> availableEnemy = new List<int>();
        if (!WaveManager.currentWave.isFinishNormalEnemy) availableEnemy.Add((int)enemyType.normalEnemy);
        if (!WaveManager.currentWave.isFinishFastEnemy) availableEnemy.Add((int)enemyType.fastEnemy);
        if (!WaveManager.currentWave.isFinishBigEnemy) availableEnemy.Add((int)enemyType.bigEnemy);

        if (availableEnemy.Count > 0)
        {         
            //within the available enemy list, we random to determine which enemy type to spawn (getting the enemy index)
            int prefabToSpawnIndex = Random.Range(0, availableEnemy.Count);
            int posToSpawn = Random.Range(0, spawnPoint.Length);

            //use the randomed index to refer to actual enemies array to spawn enemy
            poolManager.ReuseObject(objPrefabs[availableEnemy[prefabToSpawnIndex]], spawnPoint[posToSpawn].position, spawnPoint[posToSpawn].rotation);

            //deduct the available enemy after spawning
            if (availableEnemy[prefabToSpawnIndex] == (int)enemyType.normalEnemy) WaveManager.currentWave.normalEnemyNum--;
            if (availableEnemy[prefabToSpawnIndex] == (int)enemyType.fastEnemy) WaveManager.currentWave.fastEnemyNum--;
            if (availableEnemy[prefabToSpawnIndex] == (int)enemyType.bigEnemy) WaveManager.currentWave.bigEnemyNum--;
        }
    }
}
