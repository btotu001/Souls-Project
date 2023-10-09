using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TT
{
    public class EnemyStatsManager : CharacterStatsManager
    {
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyBossManager enemyBossManager;
        public UIEnemyHealthBar enemyHealthBar;

        public bool isBoss;

        private void Awake()
        {
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            enemyBossManager = GetComponent<EnemyBossManager>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private void Start()
        {
            //set healthbar only for noraml enemies
            if (!isBoss)
            {
                enemyHealthBar.SetMaxHealth(maxHealth);
            }
          

        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public override void TakeDamageNoAnimation(int damage)
        {
            base.TakeDamageNoAnimation(damage);
            if (!isBoss)
            {
                enemyHealthBar.SetCurrentHealth(currentHealth);
            }
            else if(isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealthBar(currentHealth); //add maxhealth
            }
          

        }

        //override from character stats
        public override void TakeDamage(int damage, string damageAnimation = "Damage_1")
        {

            base.TakeDamage(damage, damageAnimation = "Damage_1");

            if (!isBoss)
            {
                enemyHealthBar.SetCurrentHealth(currentHealth);
            }
            else if(isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealthBar(currentHealth);
            }
            
            enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }


        public override void TakePoisonDamage(int damage)
        {
            if (isDead)
                return;

            base.TakePoisonDamage(damage);

            if (!isBoss)
            {
                enemyHealthBar.SetCurrentHealth(currentHealth);
            }
            else if (isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealthBar(currentHealth); //add maxhealth
            }
           

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                enemyAnimatorManager.PlayTargetAnimation("Death_1", true); //ADD POISON ANIMATION

                isDead = true;
            }
        }

        private void HandleDeath()
        {
            currentHealth = 0;
            enemyAnimatorManager.PlayTargetAnimation("Death_1", true);
            // HANDLE enemy DEATH
            isDead = true;

            
        }
    }

}
