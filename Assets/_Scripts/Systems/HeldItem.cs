using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// TO (potentially) DO:
///
/// Item outline on potentialPickups[0].
/// </summary>

public class HeldItem : MonoBehaviour
{
    public static HeldItem instance;
    
    [SerializeField] private bool holdingItem = false;
    public GameObject heldItem;
    public List<GameObject> potentialPickups;
    public bool canPickup = true;

    [SerializeField] private ItemDatabaseScriptableObject items;

    [SerializeField] private Material outlineShader;
    [SerializeField] private Material[] matArray;
    [SerializeField] private List<Material> matArray2;

    public GameObject inventoryItem;

    private void Awake()
    {
        instance = this;
    }

    public void PickupItem(GameObject item)
    {
        if (GameManager.instance.curRoom == GameManager.Rooms.INTRO)
        {
            if(!canPickup)  return;
        
            //If already holding an item, drop it first.
            if (heldItem)
            {
                DropItem();
            }
        
            //Disable movement while picking up.
            GameManager.instance.ToggleMovement(false);
        
            //Assign found item as currently held item.
            heldItem = item;
        
            if (heldItem.GetComponent<MeshRenderer>().materials.Length == 2)
            {
                matArray2 = matArray.ToList();
            
                matArray2.Remove(matArray2[1]);

                matArray = matArray2.ToArray();

                heldItem.GetComponent<MeshRenderer>().materials = matArray;
            }

            //Move the item in place, re-enable movement and set items transform parent.
            heldItem.transform.DOMove(transform.position, 0.75f).OnComplete
                ((() => GameManager.instance.ToggleMovement(true)));
            heldItem.transform.DORotate(transform.rotation.eulerAngles, 0.5f);
            heldItem.transform.parent = transform;
        
            //If item has a Collider and/or Rigidbody, disable them while held.
            if(heldItem.TryGetComponent(out Rigidbody itemRB))
            {
                itemRB.useGravity = false;
                itemRB.velocity = Vector3.zero;
            }

            if (heldItem.TryGetComponent(out Collider itemCollider))
            {
                itemCollider.enabled = false;
            }

            //Now holding item, yay!
            holdingItem = true;

            //If the currently held item is the same one we stored as potential pickup, reset potential pickup.
            for (int i = 0; i < potentialPickups.Count; i++)
            {
                if (heldItem == potentialPickups[i])
                {
                    potentialPickups.Remove(potentialPickups[i]);
                }
            }
        }
        

        if (GameManager.instance.curRoom == GameManager.Rooms.FARM)
        {
            if(!canPickup)  return;

            heldItem = item;
            
            //Disable movement while picking up.
            GameManager.instance.ToggleMovement(false);

            if (heldItem.GetComponent<MeshRenderer>().materials.Length == 2)
            {
                matArray2 = matArray.ToList();
            
                matArray2.Remove(matArray2[1]);

                matArray = matArray2.ToArray();

                heldItem.GetComponent<MeshRenderer>().materials = matArray;
            }

            //Move the item in place, re-enable movement and set items transform parent.
            heldItem.transform.DOMove(transform.position, 0.75f).OnComplete
                (() => GameManager.instance.ToggleMovement(true));
            heldItem.transform.DORotate(transform.rotation.eulerAngles, 0.5f);
            heldItem.transform.parent = transform;
        
            //If item has a Collider and/or Rigidbody, disable them while held.
            if(heldItem.TryGetComponent(out Rigidbody itemRB))
            {
                itemRB.useGravity = false;
                itemRB.velocity = Vector3.zero;
            }

            if (heldItem.TryGetComponent(out Collider itemCollider))
            {
                itemCollider.enabled = false;
            }
            
            for (int i = 0; i < items.ItemDatabase.Count; i++)
            {
                if (item.name == items.ItemDatabase[i].name)
                {
                    for (int j = 0; j < PlayerLogic.instance.inventory.Count; j++)
                    {
                        if (PlayerLogic.instance.inventory[j].name == string.Empty)
                        {
                            PlayerLogic.instance.inventory[j] = items.ItemDatabase[i];
                            potentialPickups.Remove(potentialPickups[0]);
                            Destroy(heldItem, 0.77f);
                            HoldInventoryItem(PlayerLogic.instance.inventory[PlayerLogic.instance.itemIndex].model);
                            return;
                        }
                    }
                }
            }
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
            potentialPickups.Add(other.gameObject);

            matArray = potentialPickups[0].GetComponent<MeshRenderer>().materials;

            matArray2 = matArray.ToList();
            
            if (matArray2.Count == 1)
            {
                matArray2.Add(outlineShader);

                matArray = matArray2.ToArray();

                potentialPickups[0].GetComponent<MeshRenderer>().materials = matArray;
            }

            potentialPickups[0].GetComponent<MeshRenderer>().materials = matArray;
        }
    }

    public void HoldInventoryItem(GameObject item)
    {
        if (heldItem != item)
        {
            Destroy(inventoryItem);
            heldItem = item;
            inventoryItem = Instantiate(heldItem, transform);
        }
        else
        {
            heldItem = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //If far away from potential item to pickup, remove it as a pickup target.
        if (other.CompareTag("PickUp"))
        {
            matArray = potentialPickups[0].GetComponent<MeshRenderer>().materials;

            matArray2 = matArray.ToList();

            if (matArray2.Count == 2)
            {
                matArray2.Remove(matArray2[1]);

                matArray = matArray2.ToArray();

                potentialPickups[0].GetComponent<MeshRenderer>().materials = matArray;
            }
            
            potentialPickups.Remove(other.gameObject);
        }
    }
    
    private void Update()
    {
        //Pick up the potential pick up target.
        if (Input.GetKeyUp(KeyCode.E))
        {
            if(potentialPickups.Count >= 1) {PickupItem(potentialPickups[0]);}
        }
        
        //Drop currently held item.
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (holdingItem)
            {
                DropItem();
            }
        }
    }
}
