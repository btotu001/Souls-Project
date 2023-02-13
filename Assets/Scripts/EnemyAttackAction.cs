using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    [CreateAssetMenu(menuName = "A.I/Enemy Actions/Attack Action")]
    public class EnemyAttackAction : EnemyActions
    {
        public bool canCombo; //can combo after this attack action?

        public EnemyAttackAction comboAction;

        public int attackScore = 3;
        public float recoveryTime = 3;

        public float maximumAttackAngle = 35;
        public float minimumAttackAngle = -35;


        public float minimumDistanceNeededToAttack = 0;
        public float maximumDistanceNeededToAttack = 3;
    }
}
