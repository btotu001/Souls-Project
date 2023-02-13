using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventory playerInventory;
        public BlockingCollider blockingCollider;

        private void Awake()
        {
            inputHandler = GetComponentInParent<InputHandler>();    
            playerInventory = GetComponentInParent<PlayerInventory>();
            
        }

        public void OpenBlockingCollider()
        {
            //if player is two handing the right weapon, that weapon becomes "shield" when blocking
            if (inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsoption(playerInventory.rightWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsoption(playerInventory.leftWeapon);
            }
           
            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}
