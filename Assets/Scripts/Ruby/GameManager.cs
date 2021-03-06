﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static float deltaTime;
    public static float timeScale = 1;

    public static List<Transform> friendlies = new List<Transform>();

    public GameObject winTxt;
    public GameObject loseTxt;
    public GameObject PausePanel;

    public Animator fadeAnim;

    private void Start()
    {
        friendlies.Clear();
        //adding all friendly object's transform into list for pathfinding reference
        GameObject[] friendlyObj = GameObject.FindGameObjectsWithTag("FRIENDLIES");
        for (int x = 0; x < friendlyObj.Length; x++)
            friendlies.Add(friendlyObj[x].transform);
    }
    void Update ()
    {
        deltaTime = Time.deltaTime * timeScale;

        //check pause input
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (PausePanel.activeSelf)
            {
                PausePanel.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                PausePanel.SetActive(true);
                Time.timeScale = 0;
            }
        }

        //check exit input (when end game e.g. win or lose)
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            if (winTxt.activeSelf || loseTxt.activeSelf)
            {
                //go back Main Menu scene
                AudioSource music = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
                music.clip = music.gameObject.GetComponent<BGM>().menuBGM;
                music.Play();

                fadeAnim.SetTrigger("fadeOut");
                StartCoroutine(onFadeComplete());
            }
        }
	}

    IEnumerator onFadeComplete()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
    public void winGame()
    {
        if (!winTxt.activeSelf)
            winTxt.SetActive(true);        
    }

    public void loseGame()
    {
        loseTxt.SetActive(true);
        timeScale = 0;
    }
}
