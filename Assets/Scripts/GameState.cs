using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    public int BankBalance = 0;
    public int HighestBalance = 0;
    public double ElapsedSeconds = 0;

    public void CheckBalance()
    {
        // Update highscore
        if (BankBalance > HighestBalance) {
            HighestBalance = BankBalance;
        }

        if (BankBalance < 0)
        {
            SceneManager.LoadScene("Done");
        }
    }


}
