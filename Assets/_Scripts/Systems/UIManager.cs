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

        if (DialogueManager.instance.curRoom == DialogueManager.Rooms.INTRO)
        { 
            centerUIDot.GetComponent<CanvasGroup>().alpha = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void NextSubtitle(string subtitle)
    {
        dialogue.text = subtitle;
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
