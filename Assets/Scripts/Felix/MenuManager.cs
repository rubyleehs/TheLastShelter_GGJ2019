using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayButtonClicked()
    {
        SceneManager.LoadScene("Gameplay Scene", LoadSceneMode.Single);
        print("haha");          
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
