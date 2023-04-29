using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    [SerializeField]
    GameObject _dialog;

    [SerializeField]
    TextMeshProUGUI _message;

    static Dialog _self;

    void Awake()
    {
        _self = this;
        _dialog.SetActive(false);
    }

    public static void Show(string msg)
    {
        _self._message.text = msg;
        _self._dialog.SetActive(true);
    }
}
