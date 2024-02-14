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
        
        /*if (!lineType.condition)
        {
            InvokeRepeating("CheckForCondition", 0f, 0.25f);
        }*/
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
            
            NextLine(conditionLine);
            StopCoroutine(CheckForCondition(conditionLine));
        }

        if (condition == 2)
        {
            Debug.Log("Condition Shaking");

            lineIndex++;
            performedCondition = true;
            checkingCondition = false;
            
            NextLine(conditionLine);
            StopCoroutine(CheckForCondition(conditionLine));
        }

        yield return new WaitForSeconds(0.25f);
        if (performedCondition == false)
        {
            StartCoroutine(CheckForCondition(conditionLine));
        }
        else
        {
            //StopCoroutine(CheckForCondition(conditionLine));
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

        if (dialogueBank.introLines[lineIndex].condition)
        {
            if (!checkingCondition)
            {
                StartCoroutine(CheckForCondition(dialogueBank.introLines[lineIndex]));
                checkingCondition = true;
            }

            while (checkingCondition)
            {
                elapsedTime = 0;
            }
        }

        if (elapsedTime >= dialogueBank.introLines[lineIndex].duration)
        {
            if (lineIndex > 0)
            {
                
            }
            else
            {
                NextLine(dialogueBank.introLines[lineIndex]);
                lineIndex++;
                elapsedTime = 0f;
            }
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
