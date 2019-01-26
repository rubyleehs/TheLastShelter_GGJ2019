using System.Collections.Generic;
using UnityEngine;
using TMPro;

public struct Wave
{
    public int normalEnemyNum;
    public bool isFinishNormalEnemy;

    public int fastEnemyNum;
    public bool isFinishFastEnemy;

    public int bigEnemyNum;
    public bool isFinishBigEnemy;

    public float spawnSpeed;

    public Wave(int normalEnemyNum, int fastEnemyNum, int bigEnemyNum, float spawnSpeed)
    {
        this.normalEnemyNum = normalEnemyNum;        
        this.fastEnemyNum = fastEnemyNum;
        this.bigEnemyNum = bigEnemyNum;

        isFinishNormalEnemy = isFinishFastEnemy = isFinishBigEnemy = false;
        this.spawnSpeed = spawnSpeed;
    }    
}


public class WaveManager : MonoBehaviour
{
    public static int totalEnemy;               //this is to keep track whether all enemies is dead in this particular wave
    public static bool isWaveFinish = false;    //to trigger the transition between waves
    public static float transitionTime = 10;    //the exact amount of time during transition between waves (should pause during upgrades window)
    static float countdownTimer;                //used to countdown the transition time


    public List<Wave> waves = new List<Wave>(); //the wave list 
    public static Wave currentWave;             //referring the current wave object, used to spawn enemy
    public static int currentWaveIndex = 0;     //referring the index, for UI purpose

    //Text References
    public TextMeshProUGUI waveTxt;
    public TextMeshProUGUI transitionTxt;
    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        waves.Add(new Wave(1,1, 1, 3));
        waves.Add(new Wave(10, 3, 0, 2.5f));
        waves.Add(new Wave(15, 8, 1, 2));
        waves.Add(new Wave(20, 15, 5, 1.5f));
        waves.Add(new Wave(30, 15, 10, 1));

        currentWave = waves[currentWaveIndex];
        totalEnemy = currentWave.normalEnemyNum + currentWave.fastEnemyNum + currentWave.bigEnemyNum;
        waveTxt.SetText("Wave: " + (currentWaveIndex + 1).ToString());
        countdownTimer = transitionTime;
    }

    // Update is called once per frame
    void Update()
    {       
        //if all enemies are defeated in this wave, wave finish
        if (totalEnemy <= 0)
            isWaveFinish = true;
        else
        {
            if (currentWave.normalEnemyNum <= 0) currentWave.isFinishNormalEnemy = true;
            if (currentWave.fastEnemyNum <= 0) currentWave.isFinishFastEnemy = true;
            if (currentWave.bigEnemyNum <= 0) currentWave.isFinishBigEnemy = true;
        }

        //if wave finished,
        if (isWaveFinish)
        {
            //check if this wave is last wave
            if (currentWaveIndex + 1 == waves.Count)
            {
                //if yes, win game!                
                gm.winGame();
                return;
            }
            //start countdown transition
            if (countdownTimer > 0)
            {
                if (!transitionTxt.gameObject.activeSelf)
                    transitionTxt.gameObject.SetActive(true);
                transitionTxt.SetText("Next wave in: " + ((int)countdownTimer).ToString());
                countdownTimer -= GameManager.deltaTime;
            }                
            else
            {
                isWaveFinish = false;
                transitionTxt.gameObject.SetActive(false);
                countdownTimer = transitionTime;
                currentWaveIndex++;
                currentWave = waves[currentWaveIndex];
                waveTxt.SetText("Wave: " + (currentWaveIndex + 1).ToString());
                totalEnemy = currentWave.normalEnemyNum + currentWave.fastEnemyNum + currentWave.bigEnemyNum;
            }
        }
    }   
}
