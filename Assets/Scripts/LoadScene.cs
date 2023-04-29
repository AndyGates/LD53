using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField]
    string _sceneName = "Game";

    public void Load()
    {
        SceneManager.LoadScene(_sceneName);
    }
}
