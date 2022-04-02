using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    public string SceneName;
    public float Delay = 0;

    public void Execute()
    {
        if(Delay == 0)
        {
            SceneManager.LoadScene(SceneName);
        }
        else
        {
            StartCoroutine(LoadSceneInSeconds());
        }
    }

    IEnumerator LoadSceneInSeconds()
    {
        yield return new WaitForSeconds(Delay);
        SceneManager.LoadScene(SceneName);
    }
}
