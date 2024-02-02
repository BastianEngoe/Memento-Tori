using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    [SerializeField] private TMP_Text dialogue;
    [SerializeField] private DialogueBankScriptableObject dialogueBank;
    private float elapsedTime;
    private int lineIndex, eventIndex;
    public enum Rooms
    {
        INTRO,
        FARM,
        RACE,
        BLOCK,
        SHOOTER
    }

    public Rooms curRoom;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        dialogue.text = null;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        switch (curRoom)
        {
            case Rooms.INTRO: UpdateIntroRoom();
                break;
            
            case Rooms.FARM: UpdateFarmRoom();
                break;
            
            case Rooms.RACE: UpdateRaceRoom();
                break;
            
            case Rooms.BLOCK: UpdateBlockRoom();
                break;
            
            case Rooms.SHOOTER: UpdateShooterRoom();
                break;
        }
    }

    void NextLine(DialogueBankScriptableObject.DialogueLine lineType, AudioSource audioSource)
    {
        dialogue.text = lineType.dialogue;
        if (audioSource)
        {
            audioSource.clip = lineType.voiceline;
            audioSource.Play();
        }

        if (lineType.triggerEvent)
        {
            Debug.Log("Event triggered from " + lineType);
            EventManager.instance.TriggerEvent(lineType, eventIndex);
            eventIndex++;
        }
    }

    private void UpdateIntroRoom()
    {
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
            NextLine(dialogueBank.introLines[lineIndex], null);
            lineIndex++;
            elapsedTime = 0f;
        }
    }
    
    private void UpdateFarmRoom()
    {
        if (lineIndex == dialogueBank.farmLines.Count)
        {
            return;
        }
        
        if (dialogueBank.farmLines[lineIndex].duration == 0)
        {
            dialogueBank.farmLines[lineIndex].duration = 3f;
        }

        if (elapsedTime >= dialogueBank.farmLines[lineIndex].duration)
        {
            NextLine(dialogueBank.farmLines[lineIndex], null);
            lineIndex++;
            elapsedTime = 0f;
        }
    }
    
    private void UpdateRaceRoom()
    {
        if (lineIndex == dialogueBank.raceLines.Count)
        {
            return;
        }
        
        if (dialogueBank.raceLines[lineIndex].duration == 0)
        {
            dialogueBank.raceLines[lineIndex].duration = 3f;
        }

        if (elapsedTime >= dialogueBank.raceLines[lineIndex].duration)
        {
            NextLine(dialogueBank.raceLines[lineIndex], null);
            lineIndex++;
            elapsedTime = 0f;
        }
    }
    
    private void UpdateBlockRoom()
    {
        if (lineIndex == dialogueBank.blockLines.Count)
        {
            return;
        }
        
        if (dialogueBank.blockLines[lineIndex].duration == 0)
        {
            dialogueBank.blockLines[lineIndex].duration = 3f;
        }

        if (elapsedTime >= dialogueBank.blockLines[lineIndex].duration)
        {
            NextLine(dialogueBank.blockLines[lineIndex], null);
            lineIndex++;
            elapsedTime = 0f;
        }
    }
    
    private void UpdateShooterRoom()
    {
        if (lineIndex == dialogueBank.shooterLines.Count)
        {
            return;
        }
        
        if (dialogueBank.shooterLines[lineIndex].duration == 0)
        {
            dialogueBank.shooterLines[lineIndex].duration = 3f;
        }

        if (elapsedTime >= dialogueBank.shooterLines[lineIndex].duration)
        {
            NextLine(dialogueBank.shooterLines[lineIndex], null);
            lineIndex++;
            elapsedTime = 0f;
        }
    }
}
