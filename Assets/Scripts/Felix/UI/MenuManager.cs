using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    AudioSource UIaudio;    
    AudioSource BGMManager;
    public Animator fadeAnim;

    private void Start()
    {
        DontDestroyOnLoad(this);
        
        UIaudio = GetComponent<AudioSource>();
        BGMManager = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();

    }
    public void PlayButtonClicked()
    {
        BGMManager.clip = BGMManager.gameObject.GetComponent<BGM>().gameplayBGM;
        BGMManager.Play();

        fadeAnim.SetTrigger("fadeOut");
        StartCoroutine(onFadeComplete("Gameplay Scene"));    
    }

    public void CreditButtonClicked()
    {
        fadeAnim.SetTrigger("fadeOut");
        StartCoroutine(onFadeComplete("Credit Scene"));
    }

    public void HelpButtonClicked()
    {
        fadeAnim.SetTrigger("fadeOut");
        StartCoroutine(onFadeComplete("Help Scene"));
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

    IEnumerator onFadeComplete(string sceneName)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
