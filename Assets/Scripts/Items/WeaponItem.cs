using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TT
{

    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {

        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Damage")]
        public int baseDamage = 25;
        public int criticalDamageMultiplier = 4;

        [Header("Idle AAnimations")]
        public string right_hand_idle;
        public string left_hand_idle;
        public string th_idle;

        [Header("One Handed Attack Animations")]
        // OH = one hand
        public string OH_Light_Attack_1;
        public string OH_Light_Attack_2;
        public string OH_Heavy_Attack_1;

        [Header("Two Handed Attack Animations")]
        public string TH_Light_Attack_1;
        public string TH_Light_Attack_2;

        [Header("Stamina Costs")]
        public int baseStamina;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier;

        [Header("Weapon Type")]
        public bool isMeleeWeapon;
        public bool isSpellCaster;
        public bool isFaithCaster;
        public bool isPyroCaster;

    }

}
