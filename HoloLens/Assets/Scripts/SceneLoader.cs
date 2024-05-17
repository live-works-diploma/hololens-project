using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByName()
    {
        SceneManager.LoadScene("Main");
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadOnSiteScene()
    {
        SceneManager.LoadScene("OnSite");
    }

    public void LoadOffSiteScene()
    {
        SceneManager.LoadScene("OffSite");
    }
}
