using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Console : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _output;

    [SerializeField]
    GameObject _console;

    static Console _self;

    float _visableTime = 0.0f;

    void Awake()
    {
        _self = this;
        _console.SetActive(false);
    }

    void Update()
    {
        _visableTime -= Time.deltaTime;

        if (_visableTime <= 0.0f)
        {
            _console.SetActive(false);
            _visableTime = 0.0f;
        }
    }

    public static void Show(string msg)
    {
        _self._output.text = msg;
        _self._console.SetActive(true);
        _self._visableTime = 5.0f;
    }
}
