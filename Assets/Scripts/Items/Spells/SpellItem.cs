using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class SpellItem : Item
    {
        public GameObject spellWarmUpFX;
        public GameObject spellCastFX;
        public string spellAnimation;

        [Header("Spell Cost")]
        public int focusPointCost;

        [Header("Spell Type")]
        public bool isFaithSpell;
        public bool isMagicSpell;
        public bool isPyroSpell;

        [Header("Spell Description")]
        [TextArea]
        public string spellDesc;

        //warmup phase for spell casting
        //righthandslot from WeaponSlotManager is a weapon holder slot
        public virtual void AttempToCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats, WeaponSlotManager weaponSlotManager)
        {
            Debug.Log("Attempted to cast a spell!");
        }

        public virtual void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats, CameraHandler cameraHandler, WeaponSlotManager weaponSlotManager)
        {
            Debug.Log("Successfully casted a spell!");
            playerStats.DecreaseFocusPoints(focusPointCost);
        }
    }
}
