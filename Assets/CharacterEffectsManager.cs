using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        CharacterStatsManager characterStatsManager;

        [Header("Poison")]
        public GameObject defaultPoisonFX;
        public GameObject currentPoisonFX;
        public Transform buildUpTransform; //the location build up fx will spawn
        public bool isPoisoned;
        public float poisonBuildup = 0; //The buildup oveer time thath poisons the player after reaching 100
        public float poisonAmount = 100; //The amount of poison the player has to process before becoming unpoisoned
        public float defaultPoisonAmount = 100; //The default amount of poison a player has to process once they become poisoned
        public float poisonTimer = 2; //The amount of time between each poison dmg tick
        public int poisonDamage = 1;
        float timer;


        protected virtual void Awake()
        {
            characterStatsManager = GetComponent<CharacterStatsManager>();
        }

        public virtual void HandleAllBuildUpEffects()
        {
            if (characterStatsManager.isDead)
                return;

            HandlePoisonBuildUp();
            HandleIsPoisonedEffect();
        }

        protected virtual void HandlePoisonBuildUp()
        {
            if (isPoisoned)
                return;

            if(poisonBuildup > 0 && poisonBuildup < 100)
            {
                poisonBuildup = poisonBuildup - 1 * Time.deltaTime;
            }
            else if (poisonBuildup >= 100)
            {
                isPoisoned = true;
                poisonBuildup = 0;

                if(buildUpTransform != null)
                {
                    currentPoisonFX = Instantiate(defaultPoisonFX, buildUpTransform.transform);
                }
                else
                {
                    currentPoisonFX = Instantiate(defaultPoisonFX, characterStatsManager.transform);
                }
            }
        }

        protected virtual void HandleIsPoisonedEffect()
        {
            if (isPoisoned)
            {
                if(poisonAmount > 0)
                {
                    timer += Time.deltaTime;

                    if(timer >= poisonTimer)
                    {
                        //Damage player
                        characterStatsManager.TakePoisonDamage(poisonDamage);
                        timer = 0;
                    }
                  
                    poisonAmount = poisonAmount - 1 * Time.deltaTime;
                }
                else
                {
                    isPoisoned = true;
                    poisonAmount = defaultPoisonAmount;
                    Destroy(currentPoisonFX);
                }
            }
        }
    }
}
