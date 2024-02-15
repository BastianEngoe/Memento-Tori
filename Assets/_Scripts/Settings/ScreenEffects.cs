using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ScreenEffects : MonoBehaviour
{
    [HideInInspector] public bool screenEffectsEnabled; // Variable to store the ScreenEffects value
    [SerializeField] private GameObject screenFXButton; // Reference to the ScreenFX button


    private void Start()
    {
        if (!screenFXButton)
        {
            screenFXButton = GameObject.Find("ScreenFX_Button");
        }
        
        if (!PlayerPrefs.HasKey("ScreenEffects"))
        {
            PlayerPrefs.SetInt("ScreenEffects", 1);
        }
        
        screenEffectsEnabled = PlayerPrefs.GetInt("ScreenEffects") == 1;
        
        if (!screenEffectsEnabled)
        {
            screenFXButton.GetComponent<CycleSpriteOnClick>().InitialCycleOnPlayerPrefCheck();
        }
    }
    
    public void ToggleScreenFX()
    {
        // Toggle the value
        screenEffectsEnabled = !screenEffectsEnabled;
        
        // Save the value
        SetScreenFX();
    }

    private void SetScreenFX() // Method to save the ScreenEffects value to PlayerPrefs
    {
        PlayerPrefs.SetInt("ScreenEffects", screenEffectsEnabled ? 1 : 0);
    }
}
