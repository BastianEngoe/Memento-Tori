using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TheDarkness : MonoBehaviour
{
    [SerializeField] private AudioClip bulbClip;
    [SerializeField] private TMP_Text dialogue;
    [SerializeField] private DialogueBankScriptableObject dialogueBank;
    private float elapsedTime;
    private int lineIndex, eventIndex, fadeOutCounter;

    private void Start()
    {
        dialogue.text = null;
    }


    private void Update()
    {
        

        if (GetComponent<CanvasGroup>().alpha >= 0.99f)
        {
            elapsedTime += Time.unscaledDeltaTime;
            
            if (lineIndex == dialogueBank.introLines.Count)
            {
                return;
            }
        
            if (dialogueBank.introLines[lineIndex].duration == 0)
            {
                dialogueBank.introLines[lineIndex].duration = 3f;
            }

            if (elapsedTime >= dialogueBank.introLines[lineIndex].duration)
            {
                if (!dialogueBank.introLines[lineIndex].condition)
                {
                    return;
                }
                NextLine(dialogueBank.introLines[lineIndex]);
                lineIndex++;
                elapsedTime = 0f;
            }
        }
    }
    
    void NextLine(DialogueBankScriptableObject.DialogueLine lineType)
    {
        dialogue.text = lineType.dialogue;
        // if (lineType.voiceline)
        // {
        //     GameManager.instance.mascotSpeaker.clip = lineType.voiceline;
        //     GameManager.instance.mascotSpeaker.Play();
        //     lineType.duration = lineType.voiceline.length;
        // }

        if (lineType.triggerEvent)
        {
            FadeOut();
            lineIndex++;
        }

        // if (!lineType.condition)
        // {
        //     InvokeRepeating("CheckForCondition", 0f, 0.25f);
        // }
    }

    public void InvokeTheDarkness()
    {
        StartCoroutine("PlayAudioClip");
    }

    private IEnumerator PlayAudioClip()
    {
        GetComponent<AudioSource>().PlayOneShot(bulbClip);
        GetComponent<Animator>().SetTrigger("Dark");
        yield return new WaitForSecondsRealtime(0.2f);
        GetComponent<AudioSource>().PlayOneShot(bulbClip);
        yield break;
    }

    private void FadeOut()
    {
        fadeOutCounter++;
        // StartCoroutine("FadeOut");
        GetComponent<Animator>().SetTrigger("Light");
        GetComponent<Animator>().ResetTrigger("Dark");
        EventSystem.current.SetSelectedGameObject(null);
        
        if (fadeOutCounter > 2)
        {
            lineIndex = 3;
        }
    }
    
    

    
}
