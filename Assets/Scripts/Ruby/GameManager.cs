using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static float deltaTime;
    protected float timeScale = 1;

    public List<Transform> friendlies;

	void Update ()
    {
        deltaTime = Time.deltaTime * timeScale;
	}
}
