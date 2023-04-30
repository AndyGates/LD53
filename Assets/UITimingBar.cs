using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimingBar : MonoBehaviour
{
    float _time = 1.0f;

    [SerializeField]
    Image _timeBarSprite;

    //Progress 0-1
    public float Time 
    { 
        get { return _time; }
        set 
        {
            _time = value;
            UpdateSprite();
        }
    }

    void UpdateSprite()
    {        
        //Scale the bar x from 0 - 1 
        Vector3 newScale = _timeBarSprite.transform.localScale;
        newScale.x = _time;
            
        _timeBarSprite.transform.localScale = newScale; 
    }
}
