using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    [Header("Audio")]
    AudioSource audio;
    public AudioClip waveVictory;
    public AudioClip waveTransition;

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

    //transition FillAmount references
    public Image[] transitionFill;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();

        waves.Add(new Wave(20, 0, 1, 3));
        waves.Add(new Wave(30, 5, 3, 2));
        waves.Add(new Wave(30, 20, 10, 1.5f));
        waves.Add(new Wave(40, 25, 20, 1f));
        waves.Add(new Wave(50, 30, 25, 0.5f));
        waves.Add(new Wave(60, 50, 30, 0.2f));

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
        {
            isWaveFinish = true;
        }
            
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
                {
                    transitionTxt.gameObject.SetActive(true);
                    audio.clip = waveVictory;
                    audio.Play();
                }
                    
                transitionTxt.SetText("Next wave in: " + ((int)countdownTimer).ToString());
                countdownTimer -= GameManager.deltaTime;
                for (int x = 0; x < transitionFill.Length; x++)
                {
                    transitionFill[x].fillAmount = (countdownTimer / 10);
                }

            }                
            else
            {
                audio.clip = waveTransition;
                audio.Play();
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
