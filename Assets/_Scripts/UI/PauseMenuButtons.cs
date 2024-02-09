using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PauseMenuButtons : MonoBehaviour
{
    private float pauseSymbolTimeElapsed, buttonScaleTimeElapsed;
    private float pauseSymbolLerpDuration = 0.7f; //Change this to change the time it takes to flash the pause icon
    private Color pauseSymbolColor1, pauseSymbolColor2;
    private bool hoveringOverButton;
    private Vector2 scale1, scale2;
    private int timesLerpedButtonScale;
    void Start()
    {
        pauseSymbolColor1 = new Color(1, 1, 1, 0);
        pauseSymbolColor2 = new Color(1, 1, 1, 1);
        scale1 = new Vector2(1f,1f);
        scale2 = new Vector2(1.05f,1.05f);
    }

    
    private void Update()
    {
        if (name == "Paused_Icon" && Time.timeScale == 0)
        {
            PauseSymbolLerp();
        }

        if (hoveringOverButton)
        {
            HoverButtonLerp();
        }
    }
    
    
    private void PauseSymbolLerp()
    {
        if (pauseSymbolTimeElapsed < pauseSymbolLerpDuration)
        {
            GetComponent<Image>().color = Color.Lerp(pauseSymbolColor1, pauseSymbolColor2, pauseSymbolTimeElapsed / pauseSymbolLerpDuration);
            pauseSymbolTimeElapsed += Time.unscaledDeltaTime;
        }
        else 
        {
            GetComponent<Image>().color = pauseSymbolColor2;

            (pauseSymbolColor1, pauseSymbolColor2) = (pauseSymbolColor2, pauseSymbolColor1);

            pauseSymbolTimeElapsed = 0;
        }
    }


    public void QuitGame() //todo make this funny, move the quit button after the "Are you sure?"
    {
        Application.Quit();
    }

    public void OpenSettingsMenu()
    {
        
    }

    public void HoverButtonEnter()
    {
        hoveringOverButton = true;
        timesLerpedButtonScale = 0;

    }

    public void HoverButtonExit()
    {
        hoveringOverButton = false;
        GetComponent<RectTransform>().localScale = scale1;
        buttonScaleTimeElapsed = 0;
    }


    private void HoverButtonLerp()
    {
        if (buttonScaleTimeElapsed < (pauseSymbolLerpDuration / 5f)) //This lerp takes 1/3 of the time to bounce compared to the icon
        {
            GetComponent<RectTransform>().localScale = Vector2.Lerp(scale1, scale2, buttonScaleTimeElapsed / (pauseSymbolLerpDuration / 5f));
            buttonScaleTimeElapsed += Time.unscaledDeltaTime;
        }
        else 
        {
            GetComponent<RectTransform>().localScale = scale2;

            (scale1, scale2) = (scale2, scale1);

            buttonScaleTimeElapsed = 0;
            
            timesLerpedButtonScale++;

            if (timesLerpedButtonScale == 2)
            {
                hoveringOverButton = false;
            }
        }
    }

}
