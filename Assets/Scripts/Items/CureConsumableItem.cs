using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    [CreateAssetMenu(menuName = "Items/Consumables/Cure Effect")]
    public class CureConsumableItem : ConsumableItem
    {

        [Header("Recovery FX")]
        public GameObject cureConsumeFX;

        [Header("Cure FX")]
        public bool curePoison;
        //cure bleed
        //cure curse

        public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            base.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
            //instatniate flask in hand and play drink anim
            GameObject cure = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);

            //Add health or fp
            playerEffectsManager.currentParticleFX = cureConsumeFX;

            playerEffectsManager.instantiatedFXModel = cure;
            //cures poison TODO add to player death
            if (curePoison)
            {
                playerEffectsManager.poisonBuildup = 0;
                playerEffectsManager.poisonAmount = playerEffectsManager.defaultPoisonAmount;
                playerEffectsManager.isPoisoned = false;

                if(playerEffectsManager.currentPoisonFX != null)
                {
                    Destroy(playerEffectsManager.currentPoisonFX);
                }

            }

            weaponSlotManager.rightHandSlot.UnloadWeapon(); //hides current wep in hand

            //play recovery fx when/if we drink without being hit
        }
    }
}
