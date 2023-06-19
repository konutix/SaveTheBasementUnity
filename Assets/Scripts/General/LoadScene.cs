using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public static void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void Exit()
    {
        Application.Quit();
    }    

}
