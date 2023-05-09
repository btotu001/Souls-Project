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
        PlayerStats playerStats;
        WeaponSlotManager weaponSlotManager;

        public GameObject currentParticleFX; //The particles that will play of the current effect that is effecting the player (drinking estus, poison...)
        public GameObject instantiatedFXModel; //estus for example
        public int amountToBeHealed;

        private void Awake()
        {
            playerStats = GetComponentInParent<PlayerStats>();    
            weaponSlotManager = GetComponent<WeaponSlotManager>();
        }

        public void HealPlayerFromEffect()
        {
            playerStats.HealPlayer(amountToBeHealed);
            GameObject healEffect = Instantiate(currentParticleFX, playerStats.transform);
            Destroy(instantiatedFXModel.gameObject);
            weaponSlotManager.LoadBothWeaponsOnSlots();

        }
    }
}
