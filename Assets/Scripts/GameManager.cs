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

    public bool IsPaused { get; private set; }

    float _startTime = 0.0f;

    private void Awake()
    {
        var state = FindAnyObjectByType(typeof(GameState)) as GameState;
        if (state == null )
        {
            State = new GameObject().AddComponent<GameState>();
            DontDestroyOnLoad(State.gameObject);
        } else
        {
            State = state;
            if (SceneManager.GetActiveScene().name == "Game")
            {
                State.Reset();
            }
        }

        _startTime = Time.time;
    }

    public void EndGame(string reason, string title = "Game Over")
    {
        State.EndReason = reason;
        State.Title = title;
        State.ElapsedSeconds = Time.time - _startTime;
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
        IsPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        IsPaused = false;
    }
}
