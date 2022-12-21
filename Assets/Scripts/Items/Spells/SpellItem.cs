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
        public virtual void AttempToCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            Debug.Log("Attempted to cast a spell!");
        }

        public virtual void SuccessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            Debug.Log("Successfully casted a spell!");
            playerStats.DecreaseFocusPoints(focusPointCost);
        }
    }
}
