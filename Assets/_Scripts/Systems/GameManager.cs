using StarterAssets;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [HideInInspector] public GameObject player;
    private CharacterController playerCharController;
    private FirstPersonController playerFPSController;

    private void Awake()
    {
        //Set the GM instance to this script so we can reference it easily from any other script.
        instance = this;
        
        //Setting variables.
        player = GameObject.FindWithTag("Player");
        playerCharController = player.GetComponent<CharacterController>();
        playerFPSController = player.GetComponent<FirstPersonController>();
    }
    
    public void ToggleMovement(bool canMove)
    {
        //Easy to use function to toggle all movement, can be referenced from any script.
        playerCharController.enabled = canMove;
        if (canMove == false)
        {
            playerFPSController.RotationSpeed = 0f;
        }
        else
        {
            playerFPSController.RotationSpeed = 1f;
        }
    }
}
