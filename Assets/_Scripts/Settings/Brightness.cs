using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Brightness : MonoBehaviour
{
    [SerializeField] private Slider brightnessSlider;
    private Volume volumePostProcess;
    [HideInInspector] public float brightnessValue; //Only for showing in the inspector
    
    void Start()
    {
        volumePostProcess = GameObject.Find("MainCamera").GetComponent<Volume>();
        
        if (!brightnessSlider)
        {
            brightnessSlider = GameObject.Find("BrightnessSlider").GetComponent<Slider>();
        }
        
        if (!PlayerPrefs.HasKey("Brightness"))
        {
            PlayerPrefs.SetFloat("Brightness", 0);
            brightnessValue = 0;
            brightnessSlider.value = 0;
        }
        else
        {
            brightnessValue = PlayerPrefs.GetFloat("Brightness");
            AdjustBrightness(brightnessValue);
            brightnessSlider.value = brightnessValue;
        }
    }

    public void AdjustBrightness(float value)
    {
        if (volumePostProcess && volumePostProcess.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            colorAdjustments.postExposure.value = value;
            brightnessValue = value;
            PlayerPrefs.SetFloat("Brightness", value);
        }
    }
}
