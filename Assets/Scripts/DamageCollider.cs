using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TT
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
        Collider damageCollider;
        public Collider damageCollider2;
        public bool enabledOnStartup = false; //for colliders that does not open in animations (ie. fireball)

        public int currentWeaponDamage = 25;
        private InputHandler iha;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = enabledOnStartup;

            if(damageCollider2 != null)
            {
                damageCollider2.gameObject.SetActive(true);
                damageCollider2.isTrigger = true;
                damageCollider2.enabled = enabledOnStartup;
            }
 
        }
        private void Start()
        {
            iha = FindObjectOfType<InputHandler>();
        }
        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
            if (damageCollider2 != null)
            {
                damageCollider2.enabled = true;
            }
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            if (damageCollider2 != null)
            {
                damageCollider2.enabled = false;
            }
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
                            playerStats.DecreaseStamina(31);
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
                EnemyStatsManager enemyStats = collision.GetComponent<EnemyStatsManager>();
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

                if (enemyStats != null && !enemyStats.isDead)
                {
                    if (enemyStats.isBoss && (enemyStats.currentHealth < (enemyStats.maxHealth / 2) + 30 && enemyStats.currentHealth > (enemyStats.maxHealth / 2) - 30))
                    {
                        Debug.Log("VÄLI");
                        enemyStats.TakeDamageNoAnimation(currentWeaponDamage);

                    }
                    else
                    {
                        if (iha.twoHandFlag)
                        {
                            currentWeaponDamage += 5;
                        }
                        enemyStats.TakeDamage(currentWeaponDamage);
                        if (iha.twoHandFlag)
                        {
                            currentWeaponDamage -= 5;
                        }
                    }
                 
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
