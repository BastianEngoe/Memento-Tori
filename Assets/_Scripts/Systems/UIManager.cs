
using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private PlayerInputActions playerControls;
    private InputAction pauseAction;
    
    [SerializeField] private TMP_Text dialogue;
    public GameObject pausePanel, centerUIDot;
    private CursorLockMode beforePauseLockMode = CursorLockMode.Locked;
    private bool pauseMenuIsOpen;

    private void Awake()
    {
        instance = this;
        playerControls = new PlayerInputActions();
    }
    
    private void Start()
    {
       

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
        
        ShowOrHidePauseMenu(false);
        
        // if (DialogueManager.instance.curRoom == DialogueManager.Rooms.INTRO)
        // { 
        //     centerUIDot.GetComponent<CanvasGroup>().alpha = 1;
        //     Cursor.lockState = CursorLockMode.Locked;
        // }
    }

    
    private void PauseGame(bool pause)
    {


       
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
        Time.timeScale = pause ? 0 : 1;
        pausePanel.GetComponent<CanvasGroup>().alpha = pause ? 1 : 0;
        pausePanel.GetComponent<CanvasGroup>().blocksRaycasts = pause;
        pausePanel.GetComponent<CanvasGroup>().interactable = pause;
        pausePanel.GetComponent<PauseMenuButtons>().CloseSettingsMenu();
        EventSystem.current.SetSelectedGameObject(null);
        

        if (pause)
        {
            beforePauseLockMode = Cursor.lockState; //Remembers what the cursor mode was before pausing
        }

        Cursor.lockState = pause ? CursorLockMode.None : beforePauseLockMode;

        centerUIDot.GetComponent<CanvasGroup>().alpha = pause ? 0 : 1;
        
    }
    
    private void ShowOrHidePauseMenu(bool show)
    {
        pausePanel.GetComponent<CanvasGroup>().alpha = show ? 1 : 0; //A shortened if statement
        pausePanel.GetComponent<CanvasGroup>().blocksRaycasts = show;
        pausePanel.GetComponent<CanvasGroup>().interactable = show;

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
        PauseGame(!pauseMenuIsOpen);
        
        pauseMenuIsOpen = !pauseMenuIsOpen;
    }
}
