using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinBob : MonoBehaviour
{
     [SerializeField]
    float _max = 5.0f; 

    [SerializeField]
    float _speed = 1.0f;  

    float _offset = 0.0f; 

    float _initialY = 0.0f;

    void Awake()
    {
        _offset = Random.Range(0.0f, 5.0f);
        _initialY = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.localPosition;
        newPos.y = _initialY + Mathf.Sin((Time.time + _offset) * _speed) * _max;

        transform.localPosition = newPos;
    }
}
