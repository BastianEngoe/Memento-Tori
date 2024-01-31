using DG.Tweening;
using UnityEngine;

/// <summary>
/// TO (potentially) DO:
/// 
/// If there are two potential items nearby to be pickup, make it choose one based on distance.
/// Might require a light overhaul of this system.
///
/// BUG:
/// If there are two items nearby, you can initiate pick-up while picking up.
/// </summary>

public class HeldItem : MonoBehaviour
{
    [SerializeField] private bool holdingItem = false;
    public GameObject heldItem;
    private GameObject potentialPickup;
    
    public void PickupItem(GameObject item)
    {
        //If already holding an item, drop it first.
        if (heldItem)
        {
            DropItem();
        }
        
        //Disable movement while picking up.
        GameManager.instance.ToggleMovement(false);
        
        //Assign found item as currently held item.
        heldItem = item;

        //Move the item in place, re-enable movement and set items transform parent.
        heldItem.transform.DOMove(transform.position, 0.75f).OnComplete
            ((() => GameManager.instance.ToggleMovement(true)));
        heldItem.transform.DORotate(transform.rotation.eulerAngles, 0.5f);
        heldItem.transform.parent = transform;
        
        //If item has a Collider and/or Rigidbody, disable them while held.
        if(heldItem.TryGetComponent(out Rigidbody itemRB))
        {
            itemRB.useGravity = false;
        }

        if (heldItem.TryGetComponent(out Collider itemCollider))
        {
            itemCollider.enabled = false;
        }

        //Now holding item, yay!
        holdingItem = true;

        //If the currently held item is the same one we stored as potential pickup, reset potential pickup.
        if (heldItem == potentialPickup)
        {
            potentialPickup = null;
        }
    }

    public void DropItem()
    {
        //If item has Collider and Rigidbody, re-enable them.
        if(heldItem.TryGetComponent(out Rigidbody heldItemRB))
        {
            heldItemRB.useGravity = true;
        }

        if (heldItem.TryGetComponent(out Collider heldItemCollider))
        {
            heldItemCollider.enabled = true;
        }
        
        //Reset variables.    
        heldItem.transform.parent = null;
        heldItem = null;
        holdingItem = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //If we are near an item that can be picked up, set it as potential pickup target.
        if (other.CompareTag("PickUp"))
        {
            potentialPickup = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //If far away from potential item to pickup, remove it as a pickup target.
        if (other.CompareTag("PickUp"))
        {
            potentialPickup = null;
        }
    }
    
    private void Update()
    {
        //Pick up the potential pick up target.
        if (Input.GetKeyUp(KeyCode.E))
        {
            if(potentialPickup) {PickupItem(potentialPickup);}
        }
        
        //Drop currently held item.
        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (holdingItem)
            {
                DropItem();
            }
        }
    }
}
