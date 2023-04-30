using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameState State { get; private set; }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        var state = FindAnyObjectByType(typeof(GameState)) as GameState;
        if (state == null )
        {
            GameObject temp = new GameObject();
            State = temp.AddComponent<GameState>();
            DontDestroyOnLoad(State.gameObject);
        } else
        {
            State = state;
        }
    }

    public void EndGame()
    {
        SceneManager.LoadScene("Done");
    }

    public void UpdateGameState()
    {

    }


}
