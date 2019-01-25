using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static float deltaTime;
    protected float timeScale = 1;


    public static List<Transform> friendlies = new List<Transform>();


    private void Start()
    {
        //adding all friendly object's transform into list for pathfinding reference
        GameObject[] friendlyObj = GameObject.FindGameObjectsWithTag("FRIENDLIES");
        print(friendlyObj.Length);
        for (int x = 0; x < friendlyObj.Length; x++)
            friendlies.Add(friendlyObj[x].transform);

        print(friendlies.Count);
    }
    void Update ()
    {
        deltaTime = Time.deltaTime * timeScale;
	}
}
