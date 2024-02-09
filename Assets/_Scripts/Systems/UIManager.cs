using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private PlayerInputActions playerControls;
    private InputAction pauseAction;
    
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
    public GameObject pausePanel, centerUIDot;

    private void Awake()
    {
        instance = this;
        playerControls = new PlayerInputActions();
    }

    private void Start()
    {
        dialogue.text = null;

        if (centerUIDot == null)
        {
            centerUIDot = GameObject.Find("CenterUIDot");
        }
        if (pausePanel == null)
        {
            pausePanel = GameObject.Find("PausePanel");
        }
        
        pausePanel.GetComponent<CanvasGroup>().alpha = 0;
        pausePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        if (curRoom == Rooms.INTRO)
        { 
            centerUIDot.GetComponent<CanvasGroup>().alpha = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
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
        if (lineType.voiceline)
        {
            GameManager.instance.mascotSpeaker.clip = lineType.voiceline;
            GameManager.instance.mascotSpeaker.Play();
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
    
    
    public void PauseGame() //includes many "flip-flops" 
    {
        Time.timeScale = 1 - Time.timeScale; 
        pausePanel.GetComponent<CanvasGroup>().alpha = 1 - pausePanel.GetComponent<CanvasGroup>().alpha; 
        pausePanel.GetComponent<CanvasGroup>().blocksRaycasts = !pausePanel.GetComponent<CanvasGroup>().blocksRaycasts;

       
        switch (Cursor.lockState)
        {
            case CursorLockMode.Locked:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case CursorLockMode.None:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case CursorLockMode.Confined:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
            
        centerUIDot.GetComponent<CanvasGroup>().alpha = 1 - centerUIDot.GetComponent<CanvasGroup>().alpha;
        
        Debug.Log("Pause button pressed!");
    }
    
    private void OnEnable()
    {
        pauseAction = playerControls.Misc.Pause;
        pauseAction.Enable();
        pauseAction.performed += PauseKey;
    }

    private void OnDisable()
    {
        pauseAction.Disable();
    }
    

    private void PauseKey(InputAction.CallbackContext context)
    {
        PauseGame();
    }
}
