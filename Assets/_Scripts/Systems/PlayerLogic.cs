using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    //Purpose of this script is to change general player mechanics depending on the room state.

    public static PlayerLogic instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        switch (GameManager.instance.curRoom)
        {
            case GameManager.Rooms.INTRO: UpdateIntroLogic();
                break;
            
            case GameManager.Rooms.FARM: UpdateFarmLogic();
                break;
            
            case GameManager.Rooms.RACE: UpdateRaceLogic();
                break;
            
            case GameManager.Rooms.BLOCK: UpdateBlockLogic();
                break;
            
            case GameManager.Rooms.SHOOTER: UpdateShooterLogic();
                break;
        }
    }

    private void UpdateIntroLogic()
    {
        
    }

    private void UpdateFarmLogic()
    {
        
    }

    private void UpdateRaceLogic()
    {
        
    }

    private void UpdateBlockLogic()
    {
        
    }

    private void UpdateShooterLogic()
    {
        
    }
}
