using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void GoToScene(string sceneName)
    {
        Debug.Log("Heading to: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
