using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    AudioSource UIaudio;
    public GameObject BGMManager;
    AudioSource BGM;
    public AudioClip gameplayBGM;

    private void Start()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(BGMManager);
        UIaudio = GetComponent<AudioSource>();
        BGM = BGMManager.GetComponent<AudioSource>();       
    }
    public void PlayButtonClicked()
    {
        BGM.clip = gameplayBGM;
        BGM.Play();
        SceneManager.LoadScene("Gameplay Scene", LoadSceneMode.Single);        
    }

    public void CreditButtonClicked()
    {
        SceneManager.LoadScene("Credit Scene", LoadSceneMode.Single);
    }

    public void HelpButtonClicked()
    {
        SceneManager.LoadScene("Help Scene", LoadSceneMode.Single);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name == "Credit Scene" || currentScene.name == "Help Scene")
                SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        }       
    }

}
