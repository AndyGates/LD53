using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    [SerializeField]
    UIDialog _dialog;

    [SerializeField]
    TextMeshProUGUI _message;

    [SerializeField]
    GameManager _gameManager;

    static Dialog _self;

    void Awake()
    {
        _self = this;

        _dialog.OnClose += OnClosed;
    }

    public static void Show(string msg)
    {
        _self._message.text = msg;
        _self._dialog.gameObject.SetActive(true);

        _self._gameManager.Pause();
    }

    void OnClosed()
    {
        _self._gameManager.Resume();
    }
}
