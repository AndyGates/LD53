using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
 
public class CanvasScaleFactorAdjuster : MonoBehaviour
{
    public PixelPerfectCamera _mainCamera;
 
    void Start()
    {
        AdjustScalingFactor();
    }
 
    void LateUpdate()
    {
        AdjustScalingFactor();
    }
 
    void AdjustScalingFactor()
    {
        CanvasScaler scaler = GetComponent<CanvasScaler>();
        scaler.scaleFactor = _mainCamera.pixelRatio;
    }
}