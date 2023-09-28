using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class BlockingCollider : MonoBehaviour
    {
        public BoxCollider blockCollider;

        public float blockingPhysicalDamageAbsorption;

        private void Awake()
        {
            blockCollider = GetComponent<BoxCollider>();
        }

        public void SetColliderDamageAbsoption(WeaponItem weapon)
        {
            if(weapon != null)
            {
                blockingPhysicalDamageAbsorption = weapon.physicalDamageAbsorption; //gets value from weapon class
            }
        }

        public void EnableBlockingCollider()
        {
            blockCollider.enabled = true;   
        }

        public void DisableBlockingCollider()
        {
            blockCollider.enabled = false;
        }

    }
}
