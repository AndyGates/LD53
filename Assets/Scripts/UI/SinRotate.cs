using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinRotate : MonoBehaviour
{
    [SerializeField]
    float _max = 5.0f; 

    [SerializeField]
    float _speed = 1.0f;  

    float _offset = 0.0f; 

    void Awake()
    {
        _offset = Random.Range(0.0f, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newRot = transform.localEulerAngles;
        newRot.z = Mathf.Sin((Time.time + _offset) * _speed) * _max;

        transform.localRotation = Quaternion.Euler(newRot);
    }
}
