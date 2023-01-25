using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TT
{
    public class EnemyStats : CharacterStats
    {
       
        public HealthBar healthBar;

        Animator animator;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamageNoAnimation(int damage)
        {
            currentHealth = currentHealth - damage;

            healthBar.SetCurrentHealth(currentHealth);
           
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public void TakeDamage(int damage)
        {
            if (isDead)
                return;
            

            currentHealth = currentHealth - damage;

            healthBar.SetCurrentHealth(currentHealth);
            animator.Play("Damage_1");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("Death_1");
                // HANDLE enemy DEATH
                isDead = true;
            }
        }
    }

}
