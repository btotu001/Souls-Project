using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class CharacterStatsManager : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel = 10;
        public float maxStamina;
        public float currentStamina;

        public int focusLevel = 10;
        public float maxFocusPoints;
        public float currentFocusPoints;

        public int soulCount = 0;

        public int soulsAwardedOnDeath = 50;

        [Header("Armor Absorptions")]
        public float physicalDamageAbsorption;

        //Fire Absorption
        //Lightning Absorption
        //Magic
        //Dark

        public bool isDead;

        public virtual void TakeDamage(int physicalDamage, string damageAnimation = "Damage_1")
        {
            if (isDead)
                return;
            
            //for other armor part absortions, add to this
            float totalPhysicalDamageAbsorption = 1 - (1 - physicalDamageAbsorption / 100);

            physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));
            Debug.Log("Physical is " + physicalDamage);
            Debug.Log("Damage absorption is " + totalPhysicalDamageAbsorption + " %");

            float finalDamage = physicalDamage; //+ fireDamage + magicDamage + lightningDamage + darkDamage

            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage); 

            Debug.Log("Total Damage is " + finalDamage);

            //Death logic
            if(currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public virtual void TakeDamageNoAnimation(int damage)
        {
            currentHealth = currentHealth - damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public virtual void TakePoisonDamage(int damage)
        {
            currentHealth = currentHealth - damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }
    }
}
