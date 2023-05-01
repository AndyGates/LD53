using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
        [SerializeField]
        public Sprite _courierLeft;
        [SerializeField]
        public Sprite _courierRight;
        [SerializeField]
        public Sprite _courierStraight;

        public Sprite FromDirection(Vector3 direction)
        {
            Sprite newSprite = _courierStraight;
            if (direction == Vector3.left) {
                newSprite = _courierLeft;
            } else if (direction == Vector3.right)
            {
                newSprite = _courierRight;
            }
            return newSprite;
        }

    public GameState State { get; set; }

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

    public double GetElapsedMinutes()
    {
        var mins = State.ElapsedSeconds / 60;
        return Math.Round(mins, 2); // 2 decimal
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
    }
}
