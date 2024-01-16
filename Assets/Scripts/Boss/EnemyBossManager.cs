using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TT
{
    public class EnemyBossManager : MonoBehaviour
    {
        public string bossName;
     
        [SerializeField] private UIBossHealthBar bossHealthBar;
        EnemyStatsManager enemyStats;
        EnemyAnimatorManager enemyAnimatorManager;
        BossCombatStanceState bossCombatStanceState;
        //Handle switching phase
        //handle switching attack patterns

        [Header("Second Phase FX")]
        public GameObject particleFX;
        public Collider RiposteCollider;
       

        //scale))
        public Vector3 targetScale = new Vector3(1f, 1f, 1f);
        Vector3 targetScale2 = new Vector3(1.6f, 1.6f, 1.6f);
        public float duration = 4.0f;
        private float elapsedTime = 0f;
        private float startTime;
        bool redy = false;
        private void Awake()
        {
            enemyStats = GetComponent<EnemyStatsManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
        }

        private void Start()
        {
            bossHealthBar.SetBossName(bossName);    
            bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
        }

        //for scaling
        private void Update()
        {
            
            if (bossCombatStanceState.hasPhaseShifted && !redy)
            {
                // Calculate the interpolation factor based on Time.deltaTime and duration
                float t = Mathf.Clamp01(Time.deltaTime / duration);

                // Use Vector3.Lerp to smoothly interpolate between the current scale and target scale
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, t);
                if (transform.localScale.x >=  targetScale2.x)
                {
                    Debug.Log("STOOOOOOOOOOP");
                    redy = true;
                    
                }
            }
          
        }

        public void UpdateBossHealthBar(int currentHealth, int maxHealth)
        {
            bossHealthBar.SetBossCurrentHealth(currentHealth);
            if (currentHealth <= maxHealth / 2 && !bossCombatStanceState.hasPhaseShifted)
            {
                bossCombatStanceState.hasPhaseShifted = true;
            
                ShiftToSecondPhase();
            }

        }

        public void ShiftToSecondPhase()
        {
            enemyAnimatorManager.animator.SetBool("isPhaseShifting", true);
            //play anim /w an event that triggers fx + scale
            Debug.Log("phase2");
            enemyAnimatorManager.PlayTargetAnimation("Phase2", true);
            //switch attack actions
            bossCombatStanceState.hasPhaseShifted = true;
            RiposteCollider.enabled = false;
        }
    }
}
