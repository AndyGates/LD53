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

    [SerializeField]
    TextMeshProUGUI _elapsedTimeLabel;

    [SerializeField]
    TextMeshProUGUI _reasonLabel;

    [SerializeField]
    TextMeshProUGUI _titleLabel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _highScoreLabel.text = $"You reached ${_gameManager.State.HighestBalance}";
        _elapsedTimeLabel.text = $"You lived for {_gameManager.GetElapsedMinutes()} minutes";
        _reasonLabel.text = _gameManager.State.EndReason;
        _titleLabel.text = _gameManager.State.Title;
    }

    private void Awake()
    {

    }
}
