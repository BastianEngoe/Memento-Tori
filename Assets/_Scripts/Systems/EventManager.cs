using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    [SerializeField] private DialogueBankScriptableObject dialogueBank;

    public List<UnityEvent> introEvents;
    public List<UnityEvent> farmEvents;
    public List<UnityEvent> raceEvents;
    public List<UnityEvent> blockEvents;
    public List<UnityEvent> shooterEvents;

    private void Awake()
    {
        instance = this;
    }

    public void TriggerEvent(DialogueBankScriptableObject.DialogueLine eventType, int index)
    {
        //Debug.Log("Received event type " + eventType);

        if (UIManager.instance.curRoom == UIManager.Rooms.INTRO)
        {
            introEvents[index].Invoke();
        }
        
        if (UIManager.instance.curRoom == UIManager.Rooms.FARM)
        {
            farmEvents[index].Invoke();
        }
        
        if (UIManager.instance.curRoom == UIManager.Rooms.RACE)
        {
            raceEvents[index].Invoke();
        }
        
        if (UIManager.instance.curRoom == UIManager.Rooms.BLOCK)
        {
            blockEvents[index].Invoke();
        }
        
        if (UIManager.instance.curRoom == UIManager.Rooms.SHOOTER)
        {
            shooterEvents[index].Invoke();
        }
    }
}