using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleSize : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text subtitleText;
    [SerializeField] private GameObject subtitleSizeSlider, subtitlesToggleButton;


    private void Start()
    {
        if (!subtitleSizeSlider)
        {
            subtitleSizeSlider = GameObject.Find("SubtitleSlider");
        }
        if (!subtitlesToggleButton)
        {
            subtitlesToggleButton = GameObject.Find("SubtitleSize_Button");
        }
        if (!subtitleText)
        {
            subtitleText = GameObject.Find("Dialogue").GetComponent<TMP_Text>();
        }

        if (PlayerPrefs.HasKey("SubtitlesEnabled"))
        {
            // Get the saved volume from PlayerPrefs and set it to the audio mixer and the slider
            float subtitlesEnabled = PlayerPrefs.GetFloat("SubtitlesEnabled");
            subtitleText.color = subtitlesEnabled == 1 ? new Color(subtitleText.color.r, subtitleText.color.g, subtitleText.color.b, 1) : new Color(subtitleText.color.r, subtitleText.color.g, subtitleText.color.b, 0);
            subtitleSizeSlider.GetComponent<Slider>().interactable = subtitlesEnabled == 1;
            if (subtitlesEnabled == 0)
            {
                subtitlesToggleButton.GetComponent<CycleSpriteOnClick>().InitialCycleOnPlayerPrefCheck();
            }
        }
        else
        {
            PlayerPrefs.SetFloat("SubtitlesEnabled", 1);
        }
    }

    public void AdjustSubtitleSize(float value)
    {
        // Set the subtitle size
        subtitleText.fontSize = value;
    }
    
    public void ToggleSubtitles()
    {
        // Toggle the subtitle size slider
        subtitleSizeSlider.GetComponent<Slider>().interactable = !subtitleSizeSlider.GetComponent<Slider>().interactable;
        subtitleText.color = subtitleText.color.a == 0 ? new Color(subtitleText.color.r, subtitleText.color.g, subtitleText.color.b, 1) : new Color(subtitleText.color.r, subtitleText.color.g, subtitleText.color.b, 0);
    }
}
