using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    /// <summary>
    /// Handles all player effects. Poison, healing...
    /// </summary>
    public class PlayerEffectsManager : MonoBehaviour
    {
        PlayerStatsManager playerStatsManager;
        PlayerWeaponSlotManager playerWeaponSlotManager;

        public GameObject currentParticleFX; //The particles that will play of the current effect that is effecting the player (drinking estus, poison...)
        public GameObject instantiatedFXModel; //estus for example
        public int amountToBeHealed;

        private void Awake()
        {
            playerStatsManager = GetComponentInParent<PlayerStatsManager>();    
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        }

        public void HealPlayerFromEffect()
        {
            playerStatsManager.HealPlayer(amountToBeHealed);
            GameObject healEffect = Instantiate(currentParticleFX, playerStatsManager.transform);
            Destroy(instantiatedFXModel.gameObject);
            playerWeaponSlotManager.LoadBothWeaponsOnSlots();

        }
    }
}
