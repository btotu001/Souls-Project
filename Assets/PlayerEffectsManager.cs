using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    /// <summary>
    /// Handles all player effects. Poison, healing...
    /// </summary>
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        PlayerStatsManager playerStatsManager;
        PlayerWeaponSlotManager playerWeaponSlotManager;

        PoisonBuildUpBar poisonBuildUpBar;
        PoisonAmountBar poisonAmountBar;

        public GameObject currentParticleFX; //The particles that will play of the current effect that is effecting the player (drinking estus, poison...)
        public GameObject instantiatedFXModel; //estus for example
        public int amountToBeHealed;

        protected override void Awake()
        {
            base.Awake();
            playerStatsManager = GetComponentInParent<PlayerStatsManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();

            poisonBuildUpBar = FindObjectOfType<PoisonBuildUpBar>();
            poisonAmountBar = FindObjectOfType<PoisonAmountBar>();
        }

        public void HealPlayerFromEffect()
        {
            playerStatsManager.HealPlayer(amountToBeHealed);
            GameObject healEffect = Instantiate(currentParticleFX, playerStatsManager.transform);
            Destroy(instantiatedFXModel.gameObject);
            playerWeaponSlotManager.LoadBothWeaponsOnSlots();

        }
        //for shrugging
        public void ShrugEffect()
        {
            
            Destroy(instantiatedFXModel.gameObject);
            playerWeaponSlotManager.LoadBothWeaponsOnSlots();

        }

        protected override void HandlePoisonBuildUp()
        {
            if(poisonBuildup <= 0)
            {
                poisonBuildUpBar.gameObject.SetActive(false);
            }
            else
            {
                poisonBuildUpBar.gameObject.SetActive(true);
            }

            base.HandlePoisonBuildUp();
            poisonBuildUpBar.SetPoisonBuildUpAmount(Mathf.RoundToInt(poisonBuildup));
        }

        protected override void HandleIsPoisonedEffect()
        {
            if(isPoisoned == false)
            {
                poisonAmountBar.gameObject.SetActive(false);
            }
            else
            {
                poisonAmountBar.gameObject.SetActive(true);
            }
            base.HandleIsPoisonedEffect();
            poisonAmountBar.SetPoisonAmount(Mathf.RoundToInt(poisonAmount));
        }
    }
}
