using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    [SerializeField]
    GameManager _gameManager;

    void OnEnable()
    {
        _gameManager.Pause();
    }

    void OnDisable()
    {
        _gameManager.Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.gameObject.SetActive(false);
            _gameManager.Resume();
        }
    }
}
