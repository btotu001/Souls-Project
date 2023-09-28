using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TT
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
        Collider damageCollider;
        public bool enabledOnStartup = false; //for colliders that does not open in animations (ie. fireball)

        public int currentWeaponDamage = 25;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = enabledOnStartup;
 
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if(collision.tag == "Player")
            {
                PlayerStatsManager playerStats = collision.GetComponent<PlayerStatsManager>();
                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if(enemyCharacterManager != null)
                {
                    if (enemyCharacterManager.isParrying)
                    {
                        //Check here if you are parriable
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                        DisableDamageCollider(); // to prevent getting hit while succesfully parrying
                        return;
                    }
                    else if(shield != null && enemyCharacterManager.isBlocking)
                    {
                        float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;

                        if(playerStats != null)
                        {
                            playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "BlockImpact");
                            Debug.Log("blocked");
                            return; //return to not proceed to damage taking below
                        }
                    }
                }

                if (playerStats != null)
                {
                    playerStats.TakeDamage(currentWeaponDamage);
                }

            }

            if(collision.tag == "Enemy")
            {
                EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if (enemyCharacterManager != null)
                {
                    if (enemyCharacterManager.isParrying)
                    {
                        //Check here if you are parriable
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                        DisableDamageCollider(); // to prevent getting hit while succesfully parrying
                        return;
                    }
                    else if (shield != null && enemyCharacterManager.isBlocking)
                    {
                        float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;

                        if (enemyStats != null)
                        {
                            enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "BlockImpact");
                            return; //return to not proceed to damage taking below
                        }
                    }
                }

                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(currentWeaponDamage);
                }
            }

            //for illusionary wall
            if (collision.tag == "IllusionaryWall")
            {
                IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();

                illusionaryWall.wallHasBeenHit = true;
            }
        }
    }

}
