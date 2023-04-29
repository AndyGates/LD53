using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RenderScores : MonoBehaviour
{
    [SerializeField]
    GameManager _gameManager;

    [SerializeField]
    TextMeshProUGUI _highScoreLabel;

    // Start is called before the first frame update
    void Start()
    {
        //_highScoreLabel.text = $"You lived for {_gameManager.State.ElapsedSeconds}";
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_gameManager.State.ElapsedSeconds);
    }

    private void Awake()
    {
    }
}
