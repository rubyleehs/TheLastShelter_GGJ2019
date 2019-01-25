using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static float deltaTime;
    protected float timeScale = 1;

	void Update ()
    {
        deltaTime = Time.deltaTime * timeScale;
	}
}
