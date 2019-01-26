using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Wave
{
    public int normalEnemyNum;
    public bool isFinishNormalEnemy;

    public int fastEnemyNum;
    public bool isFinishFastEnemy;

    public int bigEnemyNum;
    public bool isFinishBigEnemy;

    public Wave(int normalEnemyNum, int fastEnemyNum, int bigEnemyNum)
    {
        this.normalEnemyNum = normalEnemyNum;        
        this.fastEnemyNum = fastEnemyNum;
        this.bigEnemyNum = bigEnemyNum;

        isFinishNormalEnemy = isFinishFastEnemy = isFinishBigEnemy = false;
    }    
}


public class WaveManager : MonoBehaviour
{
    public static int totalEnemy;               //this is to keep track whether all enemies is dead in this particular wave
    public static bool isWaveFinish = false;    //to trigger the transition between waves
    public static float transitionTime = 10;    //the exact amount of time during transition between waves (should pause during upgrades window)

    static float countdownTimer;                //used to countdown the transition time
    public Queue<Wave> waves = new Queue<Wave>();
    public static Wave currentWave;


    // Start is called before the first frame update
    void Start()
    {
        waves.Enqueue(new Wave(5, 0, 0));
        waves.Enqueue(new Wave(10, 3, 0));
        waves.Enqueue(new Wave(15, 8, 1));
        waves.Enqueue(new Wave(20, 15, 5));
        waves.Enqueue(new Wave(30, 15, 10));

        currentWave = waves.Dequeue();
        totalEnemy = currentWave.normalEnemyNum + currentWave.fastEnemyNum + currentWave.bigEnemyNum;
        countdownTimer = transitionTime;
    }

    // Update is called once per frame
    void Update()
    {        
        if (totalEnemy <= 0)
            isWaveFinish = true;
        else
        {
            if (currentWave.normalEnemyNum <= 0) currentWave.isFinishNormalEnemy = true;
            if (currentWave.fastEnemyNum <= 0) currentWave.isFinishFastEnemy = true;
            if (currentWave.bigEnemyNum <= 0) currentWave.isFinishBigEnemy = true;
        }

        if (isWaveFinish)
        {
            if (countdownTimer > 0)
                countdownTimer -= GameManager.deltaTime;
            else
            {
                countdownTimer = transitionTime;
                currentWave = waves.Dequeue();
                totalEnemy = currentWave.normalEnemyNum + currentWave.fastEnemyNum + currentWave.bigEnemyNum;
            }
        }
    }   
}
