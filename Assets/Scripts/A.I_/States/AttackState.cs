using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class AttackState : State
    {
        public RotateTowardsTargetState rotateTowardsTargetState;
        public CombatStanceState combatStanceState;
        public PursueTargetState pursueTargetState;
        public EnemyAttackAction currentAttack;

        bool willDoComboOnNextAttack = false;
        public bool hasPerformedAttack = false;

        /* old Tick
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            //Select one of our many attacks based on attack scores
            //if the selected attack is not able to be used because of bad angle or distance, select a new attack
            //if the attack is viable , stop our movement and attack our target
            //set our recovery timer to the attacks recovery time
            //return the combat stance state


            if (enemyManager.isInteracting && enemyManager.canDoCombo == false)
            {
                return this;
            }
            else if (enemyManager.isInteracting && enemyManager.canDoCombo)
            {
                if (willDoComboOnNextAttack)
                {
                    willDoComboOnNextAttack = false;
                    enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                }
            }
          

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            HandleRotateTowardsTarget(enemyManager);

            if (enemyManager.isPerformingAction)
            {
                return combatStanceState;

            }
           


           if (currentAttack != null)
            {
                //if we are too close to the enemy to perform current attack, get a new attack
                if(distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
                {
                    return this;
                }
                //If we are close enough to attack, then let us proceed
                else if(distanceFromTarget < currentAttack.maximumDistanceNeededToAttack)
                {
                    //If our enemy is within our attacks viewable angle, we attack
                    if(viewableAngle <= currentAttack.maximumAttackAngle && viewableAngle >= currentAttack.minimumAttackAngle)
                    {
                        if(enemyManager.currentRecoveryTime <= 0 && enemyManager.isPerformingAction == false)
                        {
                            enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                            enemyAnimatorManager.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                            enemyManager.isPerformingAction = true;
                            RollForComboChance(enemyManager);

                            if (currentAttack.canCombo && willDoComboOnNextAttack)
                            {
                                currentAttack = currentAttack.comboAction;
                                return this;
                             
                            }
                            else
                            {
                                enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                                currentAttack = null;
                                return combatStanceState;
                            }
                          
                        }
                    }

                }
            }
           else
           {
               GetNewAttack(enemyManager);
           }

           return combatStanceState;
        }
        */
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            RotateTowardsTargetWhilstAttacking(enemyManager);

            if(distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if(willDoComboOnNextAttack && enemyManager.canDoCombo)
            {
                AttackTargetWithCombo(enemyAnimatorManager, enemyManager);
               
            }

            if (!hasPerformedAttack)
            {
                AttackTarget(enemyAnimatorManager, enemyManager);
                RollForComboChance(enemyManager);
            }

            if(willDoComboOnNextAttack && hasPerformedAttack)
            {
                return this; // goes back up to perform the combo
            }

            return rotateTowardsTargetState;
        }

        private void AttackTarget(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
        {
            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformedAttack = true;
        }

        private void AttackTargetWithCombo(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
        {
            willDoComboOnNextAttack = false;
            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            currentAttack = null;
        }

        private void RotateTowardsTargetWhilstAttacking(EnemyManager enemyManager)
        {
            //Rotate manually
            if (enemyManager.canRotate && enemyManager.isInteracting)
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
           
        }

        private void RollForComboChance(EnemyManager enemyManager)
        {
            float comboChance = Random.Range(0, 100); // 100 range so it can be used as percentages

            if(enemyManager.allowAIToPerformCombos && comboChance <= enemyManager.comboLikelyHood)
            {
                if(currentAttack.comboAction != null)
                {
                    willDoComboOnNextAttack = true;
                    currentAttack = currentAttack.comboAction;
                }
                else
                {
                    willDoComboOnNextAttack=false;
                    currentAttack = null;
                }

                
            }
        }
    }
}
