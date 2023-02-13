using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class OpenChest : Interactable
    {
        Animator animator;
        OpenChest openChest;

        public Transform playerStandingPosition;
        public GameObject itemSpawner;
        public WeaponItem itemInChest;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            openChest = GetComponent<OpenChest>(); //this script
        }

        public override void Interact(PlayerManager playerManager)
        {
            //Rotate our player towards the chest
            Vector3 rotationDirection = transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            playerManager.OpenChestInteraction(playerStandingPosition);

            //Lock his transform to a certain point infront of the chest
            //open the chest lid, and animate the player
            animator.Play("Chest open");

            //spawn an item inside the chest the player can pick up
            StartCoroutine(SpawnItemInChest());
            WeaponPickup weaponPickUp = itemSpawner.GetComponent<WeaponPickup>();   

            if(weaponPickUp != null)
            {
                weaponPickUp.weapon = itemInChest;
            }

        }

        private IEnumerator SpawnItemInChest()
        {
            yield return new WaitForSeconds(1f);
            Instantiate(itemSpawner, transform);
            //Destroy this chest script so player cant interact with it again
            Destroy(openChest);
        }
    }
}
