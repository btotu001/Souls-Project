using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Lock On Transform")]
        public Transform lockOnTransform;

        [Header("Combat Colliders")]
        public CriticalDamageCollider backStabCollider;
        public CriticalDamageCollider riposteCollider;

        [Header("Combat Flags")]
        public bool canBeRiposted;
        public bool canBeParried;
        public bool isParrying;
        public bool isBlocking;

        [Header("Movement Flags")]
        public bool isRotatingWithRootMotion;
        public bool canRotate;

        [Header("Spells")]
        public bool isFiringSpell;

        //Damage will be inflicted during an animation event
        //Used in backstab or riposte anim
        public int pendingCriticalDamage;

    }
}
