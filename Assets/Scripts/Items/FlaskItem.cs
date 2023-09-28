using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    [CreateAssetMenu(menuName = "Items/Consumables/Flask")]
    public class FlaskItem : ConsumableItem
    {
        [Header("Flask type")]
        public bool estusFlask;
        public bool ashenFlask;

        [Header("RecoveryAmount")]
        public int healthRecoveryAmount;
        public int focusPointsRecoverAmount;

        [Header("Recovery FX")]
        public GameObject recoveryFX;

        public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            base.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
            //instatniate flask in hand and play drink anim
            GameObject flask = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);

            //Add health or fp
            playerEffectsManager.currentParticleFX = recoveryFX;
            playerEffectsManager.amountToBeHealed = healthRecoveryAmount;

            playerEffectsManager.instantiatedFXModel = flask;
            weaponSlotManager.rightHandSlot.UnloadWeapon(); //hides current wep in hand

            //play recovery fx when/if we drink without being hit
        }
    }
}
