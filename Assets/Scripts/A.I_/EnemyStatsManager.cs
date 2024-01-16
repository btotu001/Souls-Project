using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace TT
{
    public class EnemyStatsManager : CharacterStatsManager
    {
        EnemyManager enemyManager;
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyBossManager enemyBossManager;
        public UIEnemyHealthBar enemyHealthBar;
        public GameObject bossBar;

        public bool isBoss;
        public GameObject deathfx;
        //for demo
        InputHandler inputhandler;
        public GameObject gate;
        public TMP_Text zouls;
        public TMP_Text endzouls;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
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
            inputhandler = FindAnyObjectByType<InputHandler>();

        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public override void TakeDamageNoAnimation(int damage)
        {
            Debug.Log("damagenoanim");
            base.TakeDamageNoAnimation(damage);
            if (!isBoss)
            {
                enemyHealthBar.SetCurrentHealth(currentHealth);
            }
            else if(isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth); //add maxhealth
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
                enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);

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
                enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth); //add maxhealth
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
            StartCoroutine("deathFx");
            
        }

        public void HandleDeath2()
        {
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                //enemyAnimatorManager.PlayTargetAnimation("Death_1", true);
                // HANDLE enemy DEATH
                isDead = true;
                StartCoroutine("deathFx");
            }
          

        }

        //demo
        IEnumerator deathFx()
        {
            if (isBoss)
            {
                enemyBossManager.particleFX.SetActive(false);
            }
           yield return new WaitForSeconds(4f);
            Instantiate(deathfx, transform.position, transform.rotation);
            yield return new WaitForSeconds(1f);
            inputhandler.lockOnInput = false;
            inputhandler.lockOnFlag = false;
            inputhandler.cameraHandler.ClearLockOnTargets();
            if (isBoss)
            {
                endzouls.text = "Zoul score: " + zouls.text;
               bossBar.gameObject.SetActive(false);
                gate.SetActive(false);
            }
            Destroy(this.gameObject);
        }
    }

}
