using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Brightness : MonoBehaviour
{
    [SerializeField] private Slider brightnessSlider;
    private Volume _volume;
    
    void Start()
    {
        _volume = GameObject.Find("MainCamera").GetComponent<Volume>();
        if (!brightnessSlider)
        {
            brightnessSlider = GameObject.Find("BrightnessSlider").GetComponent<Slider>();
        }

        AdjustBrightness(brightnessSlider.value);
    }

    public void AdjustBrightness(float value)
    {
        if (_volume && _volume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            colorAdjustments.postExposure.value = value;
        }
    }
}
