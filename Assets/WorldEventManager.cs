using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    /// <summary>
    /// Main manager for events such as boss fight, item spawn...
    /// </summary>
    public class WorldEventManager : MonoBehaviour
    {
        //Fog wall
        public List<FogWall> fogWalls;
        public UIBossHealthBar bossHealthBar;
        public EnemyBossManager boss;

        public bool bossFightIsActive;  //Is currently fighting boss
        public bool bossHasBeenAwakened; //woke the boss/ watched cutscene but died
        public bool bossHasBeenDefeated; //Boss has been defeated

        private void Awake()
        {
            //bossHealthBar = FindObjectOfType<UIBossHealthBar>();
        }

        public void ActivateBossFight()
        {
            bossFightIsActive = true;
            bossHasBeenAwakened = true;
            bossHealthBar.SetUIHealthBarToActive();

            //Activate fog wall(s)
            foreach(var fogWall in fogWalls)
            {
                fogWall.ActivateFogWall();
            }

        }

        public void BossHasBeenDefeated()
        {
            bossHasBeenDefeated = true;
            bossFightIsActive = false;

            //Deactivate fogwall
            foreach (var fogWall in fogWalls)
            {
                fogWall.DeactivateFogWall();
            }
        }


    }
}
