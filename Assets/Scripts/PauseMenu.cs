using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    [SerializeField]
    GameManager _gameManager;

    [SerializeField]
    TextMeshProUGUI _elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        _elapsedTime.text = $"Elapsed time: {_gameManager.GetElapsedMinutes()} minutes";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.gameObject.SetActive(false);
        }
    }
}
