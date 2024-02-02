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

    void NextLine(DialogueBankScriptableObject.DialogueLine lineType)
    {
        dialogue.text = lineType.dialogue;
        if (lineType.voiceSource)
        {
            lineType.voiceSource.clip = lineType.voiceline;
            lineType.voiceSource.Play();
            lineType.duration = lineType.voiceline.length;
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
            if (!dialogueBank.introLines[lineIndex].condition)
            {
                return;
            }
            NextLine(dialogueBank.introLines[lineIndex]);
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

        if (elapsedTime >= dialogueBank.farmLines[lineIndex].duration)
        {
            NextLine(dialogueBank.farmLines[lineIndex]);
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
        
        if (elapsedTime >= dialogueBank.raceLines[lineIndex].duration)
        {
            NextLine(dialogueBank.raceLines[lineIndex]);
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
            NextLine(dialogueBank.blockLines[lineIndex]);
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
            NextLine(dialogueBank.shooterLines[lineIndex]);
            lineIndex++;
            elapsedTime = 0f;
        }
    }
}
