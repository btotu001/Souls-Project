using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TT
{
    public class PursueTargetState : State
    {
        public CombatStanceState combatStanceState;
        public RotateTowardsTargetState rotateTowardsTargetState;
   
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            //look targets direction
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;

            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            //set angle
            float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);


            //before navmesh resets
            HandleRotateTowardsTarget(enemyManager);

           

            if (enemyManager.isInteracting)
                return this;

            //dont move if perfoming action
            if (enemyManager.isPerformingAction)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

           
            if (distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }
           

            //Change the target
            //If within attack range switch to combat stance
            //if target is out of range, return this state and continue to chase target
            if(distanceFromTarget <= enemyManager.maximumAggroRadius)
            {
                return combatStanceState;
            }
            else
            {
                return this;
            }

        }


        private void HandleRotateTowardsTarget(EnemyManager enemyManager)
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
    }
}
