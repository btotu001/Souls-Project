using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public abstract class State : MonoBehaviour
    {
        //inheriting classes must use all functions from abstract class?
        //abstracks cant be used. need to call it from something that derives from this
        public abstract State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager);
    }
}
