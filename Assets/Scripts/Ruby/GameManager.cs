using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static float deltaTime;
    protected float timeScale = 1;

    public static List<Transform> friendlies = new List<Transform>();

    public GameObject winTxt;
    public GameObject loseTxt;
    public GameObject PausePanel;

   
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

        //check pause input
        if (Input.GetKeyDown(KeyCode.P))
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
                SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
            }
        }
	}

    public void winGame()
    {
        if (!winTxt.activeSelf)
            winTxt.SetActive(true);

        //play some particles?
    }

    public void loseGame()
    {
        loseTxt.SetActive(true);
    }
}
