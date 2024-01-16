using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public EnemyAttackAction[] enemyAttacks;
        public PursueTargetState pursueTargetState;

        protected bool randomDestinationSet = false;
        protected float verticalMovementValue = 0;
        protected float horizontalMovementValue = 0;  

        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            enemyAnimatorManager.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            enemyAnimatorManager.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            attackState.hasPerformedAttack = false;

            if (enemyManager.isInteracting)
            {
                enemyAnimatorManager.animator.SetFloat("Vertical", 0);
                enemyAnimatorManager.animator.SetFloat("Horizontal", 0);
                return this;
            }

            //if the player runs out of range, return the pursuetarget state
            if (distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                DecideCirclingAction(enemyAnimatorManager);
            }

            HandleRotateTowardsTarget(enemyManager);

            
            if(enemyManager.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                randomDestinationSet=false;
                return attackState;
            }
            //if we are in a cooldown after attacking, return this state and continue circling player
            else 
            {
                GetNewAttack(enemyManager);
               
            }
            //shouldnt happen
            return this;
        }

        protected void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            //Rotate manually
            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, enemyManager.rotationSpeed);
            }
            //Rotate with pathfinding (navMesh)
            else
            {
                //check documentation for inversetransformdirection
                Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.enemyRigidbody.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);

            }
        }

        protected void DecideCirclingAction(EnemyAnimatorManager enemyAnimatorManager)
        {
            //Circle with only forward vertical movement
            //circle with running
            WalkAroundTarget(enemyAnimatorManager);
        }

        protected void WalkAroundTarget(EnemyAnimatorManager enemyAnimatorManager)
        {
            //verticalMovementValue = Random.Range(0, 1); //only walk towards target (-1,1) - goes back and forward
            verticalMovementValue = 0.5f; //only moves forward (better)

            horizontalMovementValue = Random.Range(0, 1);

            //horizontal is always .5 or -.5
            if(horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
            {
                horizontalMovementValue = 0.5f;
            }
            else if(horizontalMovementValue >= -1 && horizontalMovementValue < 0)
            {
                horizontalMovementValue = -0.5f;
            }

        }

        #region Attacks
        protected virtual void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

            int maxScore = 0;
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (attackState.currentAttack != null)
                            return;

                        temporaryScore += enemyAttackAction.attackScore;

                        if (temporaryScore > randomValue)
                        {
                            attackState.currentAttack = enemyAttackAction;
                        }
                    }
                }
            }

        }

        #endregion
    }
}
