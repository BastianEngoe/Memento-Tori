using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    
    public enum Rooms
    {
        INTRO,
        FARM,
        RACE,
        BLOCK,
        SHOOTER
    }
    
    [Header("Room state")]
    public Rooms curRoom;
    
    [Header("Dialogue asset")]
    [SerializeField] private DialogueBankScriptableObject dialogueBank;
    [SerializeField] private float elapsedTime;
    [SerializeField] private int lineIndex, eventIndex;

    [SerializeField] private bool checkingCondition;
    [SerializeField] private bool performedCondition;
    [SerializeField] private bool nodded;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        lineIndex = 0;
        
        if (curRoom == Rooms.INTRO)
        {
            NextLine(dialogueBank.introLines[lineIndex]);
        }
        if (curRoom == Rooms.FARM)
        {
            NextLine(dialogueBank.farmLines[lineIndex]);
        }
        if (curRoom == Rooms.RACE)
        {
            NextLine(dialogueBank.raceLines[lineIndex]);
        }
        if (curRoom == Rooms.BLOCK)
        {
            NextLine(dialogueBank.blockLines[lineIndex]);
        }
        if (curRoom == Rooms.SHOOTER)
        {
            NextLine(dialogueBank.shooterLines[lineIndex]);
        }
    }

    void Update()
    {
        switch (curRoom)
        {
            case Rooms.INTRO:
                UpdateIntroRoom();
                break;

            case Rooms.FARM:
                UpdateFarmRoom();
                break;

            case Rooms.RACE:
                UpdateRaceRoom();
                break;

            case Rooms.BLOCK:
                UpdateBlockRoom();
                break;

            case Rooms.SHOOTER:
                UpdateShooterRoom();
                break;
        }
        elapsedTime += Time.deltaTime;
    }
    
    void NextLine(DialogueBankScriptableObject.DialogueLine lineType)
    {
        if (nodded)
        {
            lineIndex++;
            nodded = false;
        }
        
        UIManager.instance.NextSubtitle(lineType.dialogue);

        if (lineType.voiceline)
        {
            GameManager.instance.mascotSpeaker.clip = lineType.voiceline;
            GameManager.instance.mascotSpeaker.Play();
            lineType.duration = lineType.voiceline.length;
        }

        if (lineType.triggerEvent)
        {
            //Debug.Log("Event triggered from " + lineType);
            EventManager.instance.TriggerEvent(lineType, eventIndex);
            eventIndex++;
        }
    }
    
    private IEnumerator CheckForCondition(DialogueBankScriptableObject.DialogueLine conditionLine)
    {
        Debug.Log("Checking for condition...");
        
        int condition;
        GameManager.instance.RetrievePlayerInput(out condition);
        if (condition == 1)
        {
            Debug.Log("Condition Nodding");

            performedCondition = true;
            checkingCondition = false;
            nodded = true;

            lineIndex++;

            if (curRoom == Rooms.INTRO)
            {
                NextLine(dialogueBank.introLines[lineIndex]);
            }
            if (curRoom == Rooms.FARM)
            {
                NextLine(dialogueBank.farmLines[lineIndex]);
            }
            if (curRoom == Rooms.RACE)
            {
                NextLine(dialogueBank.raceLines[lineIndex]);
            }
            if (curRoom == Rooms.BLOCK)
            {
                NextLine(dialogueBank.blockLines[lineIndex]);
            }
            if (curRoom == Rooms.SHOOTER)
            {
                NextLine(dialogueBank.shooterLines[lineIndex]);
            }
            
            StopCoroutine(CheckForCondition(conditionLine));
        }

        if (condition == 2)
        {
            Debug.Log("Condition Shaking");

            lineIndex += 2;
            performedCondition = true;
            checkingCondition = false;
            
            if (curRoom == Rooms.INTRO)
            {
                NextLine(dialogueBank.introLines[lineIndex]);
            }
            if (curRoom == Rooms.FARM)
            {
                NextLine(dialogueBank.farmLines[lineIndex]);
            }
            if (curRoom == Rooms.RACE)
            {
                NextLine(dialogueBank.raceLines[lineIndex]);
            }
            if (curRoom == Rooms.BLOCK)
            {
                NextLine(dialogueBank.blockLines[lineIndex]);
            }
            if (curRoom == Rooms.SHOOTER)
            {
                NextLine(dialogueBank.shooterLines[lineIndex]);
            }
            
            StopCoroutine(CheckForCondition(conditionLine));
        }

        yield return new WaitForSeconds(0.25f);
        if (performedCondition == false)
        {
            StartCoroutine(CheckForCondition(conditionLine));
        }
    }
    
     private void UpdateIntroRoom()
    {
        if (lineIndex == dialogueBank.introLines.Count - 1)
        {
            return;
        }
        
        if (dialogueBank.introLines[lineIndex].duration == 0)
        {
            dialogueBank.introLines[lineIndex].duration = 3f;
        }

        if (dialogueBank.introLines[lineIndex].condition == false)
        {
            if (!checkingCondition)
            {
                StartCoroutine(CheckForCondition(dialogueBank.introLines[lineIndex]));
                performedCondition = false;
                checkingCondition = true;
            }

            if(checkingCondition)
            {
                elapsedTime = 0;
            }
        }

        if (elapsedTime >= dialogueBank.introLines[lineIndex].duration)
        {
            lineIndex++;
            NextLine(dialogueBank.introLines[lineIndex]);
            elapsedTime = 0f;
        }
    }

     private void UpdateFarmRoom()
    {
        if (lineIndex == dialogueBank.farmLines.Count - 1)
        {
            return;
        }
        
        if (dialogueBank.farmLines[lineIndex].duration == 0)
        {
            dialogueBank.farmLines[lineIndex].duration = 3f;
        }

        if (dialogueBank.farmLines[lineIndex].condition == false)
        {
            if (!checkingCondition)
            {
                StartCoroutine(CheckForCondition(dialogueBank.farmLines[lineIndex]));
                performedCondition = false;
                checkingCondition = true;
            }

            if(checkingCondition)
            {
                elapsedTime = 0;
            }
        }

        if (elapsedTime >= dialogueBank.farmLines[lineIndex].duration)
        {
            lineIndex++;
            NextLine(dialogueBank.farmLines[lineIndex]);
            elapsedTime = 0f;
        }
    }
    
    private void UpdateRaceRoom()
    {
        if (lineIndex == dialogueBank.raceLines.Count - 1)
        {
            return;
        }
        
        if (dialogueBank.raceLines[lineIndex].duration == 0)
        {
            dialogueBank.raceLines[lineIndex].duration = 3f;
        }

        if (dialogueBank.raceLines[lineIndex].condition == false)
        {
            if (!checkingCondition)
            {
                StartCoroutine(CheckForCondition(dialogueBank.raceLines[lineIndex]));
                performedCondition = false;
                checkingCondition = true;
            }

            if(checkingCondition)
            {
                elapsedTime = 0;
            }
        }

        if (elapsedTime >= dialogueBank.raceLines[lineIndex].duration)
        {
            lineIndex++;
            NextLine(dialogueBank.raceLines[lineIndex]);
            elapsedTime = 0f;
        }
    }
    
    private void UpdateBlockRoom()
    {
        if (lineIndex == dialogueBank.blockLines.Count - 1)
        {
            return;
        }
        
        if (dialogueBank.blockLines[lineIndex].duration == 0)
        {
            dialogueBank.blockLines[lineIndex].duration = 3f;
        }

        if (dialogueBank.blockLines[lineIndex].condition == false)
        {
            if (!checkingCondition)
            {
                StartCoroutine(CheckForCondition(dialogueBank.blockLines[lineIndex]));
                performedCondition = false;
                checkingCondition = true;
            }

            if(checkingCondition)
            {
                elapsedTime = 0;
            }
        }

        if (elapsedTime >= dialogueBank.blockLines[lineIndex].duration)
        {
            lineIndex++;
            NextLine(dialogueBank.blockLines[lineIndex]);
            elapsedTime = 0f;
        }
    }
    
    private void UpdateShooterRoom()
    {
        if (lineIndex == dialogueBank.shooterLines.Count - 1)
        {
            return;
        }
        
        if (dialogueBank.shooterLines[lineIndex].duration == 0)
        {
            dialogueBank.shooterLines[lineIndex].duration = 3f;
        }

        if (dialogueBank.shooterLines[lineIndex].condition == false)
        {
            if (!checkingCondition)
            {
                StartCoroutine(CheckForCondition(dialogueBank.shooterLines[lineIndex]));
                performedCondition = false;
                checkingCondition = true;
            }

            if(checkingCondition)
            {
                elapsedTime = 0;
            }
        }

        if (elapsedTime >= dialogueBank.shooterLines[lineIndex].duration)
        {
            lineIndex++;
            NextLine(dialogueBank.shooterLines[lineIndex]);
            elapsedTime = 0f;
        }
    }
}
