using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        EnemyManager enemyManager;


        protected override void Awake()
        {
            base.Awake();
            //anim derived from animatorManager
            animator = GetComponent<Animator>();
            enemyManager = GetComponent<EnemyManager>();
        }

        public void AwardSoulsOnDeath()
        {
            //if multiplayer -> Scan for every player in the scene, award them souls

            PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
            SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();

            if (playerStats != null)
            {
                playerStats.AddSouls(characterStatsManager.soulsAwardedOnDeath);

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
