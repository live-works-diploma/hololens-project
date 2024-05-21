using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByName(string name)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        SceneManager.LoadScene(name);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
