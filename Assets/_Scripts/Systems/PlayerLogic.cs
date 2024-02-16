using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLogic : MonoBehaviour
{
    //Purpose of this script is to change general player mechanics depending on the room state.

    public static PlayerLogic instance;

    [Header("Farming variables")] 
    [SerializeField] private GameObject farmUI;
    public List<Item> inventory;
    [SerializeField] private List<GameObject> inventoryUI;
    [SerializeField] private GameObject inventoryUISelection;
    public int itemIndex;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (GameManager.instance.curRoom == GameManager.Rooms.FARM)
        {
            farmUI.SetActive(true);
        }
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
        inventory.Capacity = 6;

        if (Input.mouseScrollDelta.y > 0.5f)
        {
            if (itemIndex == inventory.Count - 1)
            {
                itemIndex = 0;
            }
            else
            {
                itemIndex++;
            }
        }

        if (Input.mouseScrollDelta.y < -0.5f)
        {
            if (itemIndex == 0)
            {
                itemIndex = inventory.Count - 1;
            }
            else
            {
                itemIndex--;
            }
        }

        for (int i = 0; i < inventoryUI.Count; i++)
        {
            inventoryUI[i].GetComponent<Image>().sprite = inventory[i].icon;
        }
        
        inventoryUISelection.transform.position = inventoryUI[itemIndex].transform.position;
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
