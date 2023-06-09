using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    private int _balance;
    public int BankBalance {
        get { return _balance; }
        set {
            _balance = value;
            // Update highscore
            if (BankBalance > HighestBalance)
            {
                HighestBalance = BankBalance;
            }
        }
    }
    public int HighestBalance = 0;
    public float ElapsedSeconds = 0;

    public string EndReason = "";
    public string Title = "Game Over";

    internal void Reset()
    {
        BankBalance = 0;
        HighestBalance = 0;
        ElapsedSeconds = 0;
        EndReason = "";
        Title = "GameOver";
    }
}
