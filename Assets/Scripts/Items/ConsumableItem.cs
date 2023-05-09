using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class ConsumableItem : Item
    {
        [Header("Item Quantity")]
        public int maxItemAmount;
        public int currentItemAmount;

        [Header("Item Model")]
        public GameObject itemModel;

        [Header("Animations")]
        public string consumeAnimation;
        public bool isInteracting;

        public virtual void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, WeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            //if player has item
            if(currentItemAmount > 0)
            {
                playerAnimatorManager.PlayTargetAnimation(consumeAnimation, isInteracting, true);
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation("Shrugging", true);
            }
        }
    }
}
