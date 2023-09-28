using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    [CreateAssetMenu(menuName = "Spells/Projectile Spell")]
    public class ProjectileSpell : SpellItem
    {
        [Header("Projectile Damage")]
        public float baseDamage;

        [Header("Projectile Physics")]
        public float projectileForwardVelocity;
        public float projectileUpwardVelocity;
        public float projectileMass;
        public bool gravity;
        Rigidbody rigidbody;

       
        public override void AttempToCastSpell(PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats, PlayerWeaponSlotManager weaponSlotManager)
        {
            base.AttempToCastSpell(animatorHandler, playerStats, weaponSlotManager);
            //instantiate the spell in the casting hand(right only for now) of player
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, weaponSlotManager.rightHandSlot.transform);
            //Play animation to cast the spell
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
        }

        public override void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats, CameraHandler cameraHandler, PlayerWeaponSlotManager weaponSlotManager)
        {
            base.SuccessfullyCastSpell(animatorHandler, playerStats, cameraHandler, weaponSlotManager);
            GameObject instantiatedSpellFX = Instantiate(spellCastFX, weaponSlotManager.rightHandSlot.transform.position, cameraHandler.cameraPivotTransform.rotation);
            rigidbody = instantiatedSpellFX.GetComponent<Rigidbody>();
            //spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();

            //aim the spell at locked on target
            if(cameraHandler.currentLockOnTarget != null)
            {
                instantiatedSpellFX.transform.LookAt(cameraHandler.currentLockOnTarget.transform);
            }
            //else aim at the players forward rotation(y), height rotation depending on camera position(x), z = 0
            else
            {
                instantiatedSpellFX.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerStats.transform.eulerAngles.y, 0);
            }

            //make the projectile move with AddForce
            rigidbody.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
            rigidbody.AddForce(instantiatedSpellFX.transform.up * projectileUpwardVelocity);
            rigidbody.useGravity = gravity;
            rigidbody.mass = projectileMass;
            instantiatedSpellFX.transform.parent = null;

        }
    }
}
