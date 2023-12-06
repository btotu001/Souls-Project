using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TT
{
    //Inherits from Interactable
    public class WeaponPickup : Interactable
    {
        public WeaponItem weapon;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            //Pick up the item and add it to the players inventory
            PickupItem(playerManager);
        }

        private void PickupItem(PlayerManager playerManager)
        {
            PlayerInventoryManager playerInventory;
            PlayerLocomotionManager playerLocomotion;
            PlayerAnimatorManager animatorHandler; 

            playerInventory = playerManager.GetComponent<PlayerInventoryManager>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotionManager>();
            animatorHandler = playerManager.GetComponentInChildren<PlayerAnimatorManager>();

            playerLocomotion.rigidbody.velocity = Vector3.zero; // Stops the player from moving whilst picking up item
            animatorHandler.PlayTargetAnimation("Pickup", true); // Plays the animation of looting the item
            playerInventory.weaponsInventory.Add(weapon); //add weapon to weaponinventory list
            playerManager.itemInteractableObject.GetComponentInChildren<TextMeshProUGUI>().text = weapon.itemName;
            playerManager.itemInteractableObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture; //change image to (weapon)items icon
            playerManager.itemInteractableObject.SetActive(true);
            //playerManager.interactableUIGameObject.SetActive(false);
            keyIcon keyicon = FindObjectOfType<keyIcon>();
            keyicon.GotKey();
            SPAWN.instance.hasKey = true;
            Destroy(gameObject); //destroy the picked item

        }
    }
}
