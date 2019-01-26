﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static float deltaTime;
    protected float timeScale = 1;

    public static List<Transform> friendlies = new List<Transform>();

    public GameObject winTxt;
    
    private void Start()
    {
        //adding all friendly object's transform into list for pathfinding reference
        GameObject[] friendlyObj = GameObject.FindGameObjectsWithTag("FRIENDLIES");
        for (int x = 0; x < friendlyObj.Length; x++)
            friendlies.Add(friendlyObj[x].transform);
    }
    void Update ()
    {
        deltaTime = Time.deltaTime * timeScale;
	}

    public void winGame()
    {
        if (!winTxt.activeSelf)
            winTxt.SetActive(true);

        //play some particles?
    }
}
