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
            if(currentItemAmount > 0)
            currentItemAmount--;

            UIManager uimanager;
            uimanager = FindObjectOfType<UIManager>();
            uimanager.flask_text.text = currentItemAmount.ToString();


            //cure poison too for demo
            playerEffectsManager.poisonBuildup = 0;
            playerEffectsManager.poisonAmount = playerEffectsManager.defaultPoisonAmount;
            playerEffectsManager.isPoisoned = false;

            if (playerEffectsManager.currentPoisonFX != null)
            {
                Destroy(playerEffectsManager.currentPoisonFX);
            }


        }
        public void RestoreFlask()
        {
           currentItemAmount = maxItemAmount;

            UIManager uimanager;
            uimanager = FindObjectOfType<UIManager>();
            uimanager.flask_text.text = currentItemAmount.ToString();
        }
    }
}
