using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class EnemyBossManager : MonoBehaviour
    {
        public string bossName;

        [SerializeField]
        private UIBossHealthBar bossHealthBar;
        EnemyStats enemyStats;
        //Handle switching phase
        //handle switching attack patterns

        private void Awake()
        {
            enemyStats = GetComponent<EnemyStats>();
        }

        private void Start()
        {
            bossHealthBar.SetBossName(bossName);    
            bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
        }

        public void UpdateBossHealthBar(int currentHealth)
        {
            bossHealthBar.SetBossCurrentHealth(currentHealth);
        }
    }
}
