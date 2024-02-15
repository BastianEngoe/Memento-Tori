using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicLevel : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer; // Reference to the AudioMixer
    private bool MusicisMuted; // Flag to track mute state of audio mixer groups
    private float volumeBeforeMute;
    [SerializeField] private GameObject musicSlider, musicToggleButton;


    private void Start()
    {

        if (!musicSlider)
        {
            musicSlider = GameObject.Find("MusicSlider");
        }
        if (!musicToggleButton)
        {
            musicToggleButton = GameObject.Find("Music_Button");
        }
        
        if (audioMixer == null)
        {
            Debug.LogWarning("AudioMixer reference is not set in this GameObject " + gameObject.name);
            
            audioMixer = Resources.Load<AudioMixer>("MasterMixer"); //find and assign the audio mixer
            if (audioMixer == null)
            {
                Debug.LogError("Failed to load AudioMixer. Please ensure the AudioMixer is located in the Resources folder and the name is correct.");
            }
        }
        
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            // Get the saved volume from PlayerPrefs and set it to the audio mixer and the slider
            float volume = PlayerPrefs.GetFloat("MusicVolume");
            audioMixer.SetFloat("MusicVolume", volume);
            musicSlider.GetComponent<Slider>().value = volume;
            
            if (volume <= -39f)
            {
                MusicisMuted = true;
                musicSlider.GetComponent<Slider>().interactable = false;
                musicToggleButton.GetComponent<CycleSpriteOnClick>().InitialCycleOnPlayerPrefCheck();
            }
        }
        else
        {
            PlayerPrefs.SetFloat("MusicVolume", 0);
            audioMixer.SetFloat("MusicVolume", 0);
            musicSlider.GetComponent<Slider>().value = 0;
        }
        
        
    }

    public void ToggleMusic()
    {
        
        
        if (!MusicisMuted)
        {
            audioMixer.GetFloat("MusicVolume", out volumeBeforeMute); // Save the volume before muting, so unmuting goes back to it
        }
        
        MusicisMuted = !MusicisMuted; // Toggle the mute state
        
        musicSlider.GetComponent<Slider>().interactable = !MusicisMuted;
        
        // Set the volume of the Mixer group based on the mute state
        float volume = MusicisMuted ? -80f : volumeBeforeMute; // Mute volume is typically set to -80dB
        audioMixer.SetFloat("MusicVolume", volume); // Set the volume parameter of the Mixer group
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    
    public void AdjustMusicLevel(float value)
    {
        audioMixer.SetFloat("MusicVolume", value);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
}
