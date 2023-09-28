using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        EnemyManager enemyManager;
        EnemyStats enemyStats;

        private void Awake()
        {
            //anim derived from animatorManager
            animator = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
            enemyStats = GetComponentInParent<EnemyStats>();
        }

        public override void TakeCriticalDamageAnimationEvent()
        {
            enemyStats.TakeDamageNoAnimation(enemyManager.pendingCriticalDamage);
            enemyManager.pendingCriticalDamage = 0; // reset pending dmg
        }

        public void CanRotate()
        {
            animator.SetBool("canRotate", true);
        }

        public void StopRotation()
        {
            animator.SetBool("canRotate", false);
        }

        public void EnableCombo()
        {
            animator.SetBool("canDoCombo", true);
        }
        public void DisableCombo()
        {
            animator.SetBool("canDoCombo", false);
        }

        public void EnableIsInvulnerable()
        {
            animator.SetBool("isInvulnerable", true);
        }

        public void DisableIsVulnerable()
        {
            animator.SetBool("isInvulnerable", false);
        }

        
        public void EnableIsParrying()
        {
            enemyManager.isParrying = true;
        }

        public void DisableIsParrying()
        {
            enemyManager.isParrying = false;
        }

        public void EnableCanBeRiposted()
        {
            enemyManager.canBeRiposted = true;
        }

        public void DisableCanBeRiposted()
        {
            enemyManager.canBeRiposted = false;
        }

        public void AwardSoulsOnDeath()
        {
            //if multiplayer -> Scan for every player in the scene, award them souls

            PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
            SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();

            if (playerStats != null)
            {
                playerStats.AddSouls(enemyStats.soulsAwardedOnDeath);

                if (soulCountBar != null)
                {
                    soulCountBar.SetSoulCountText(playerStats.soulCount);
                }
            }
          
        }

        //every time our animator plays animation with root motion, it recenters our model back on the gameobject
        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidbody.velocity = velocity;

            if (enemyManager.isRotatingWithRootMotion)
            {
                // *= so it rotates over time
                enemyManager.transform.rotation *= animator.deltaRotation; 
            }
        }
    }
}
