using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TT
{
    public class EnemyManager : CharacterManager
    {

        EnemyLocomotionManager enemyLocomotionManager;
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyStats enemyStats;

        public State currentState;
        public CharacterStats currentTarget;

        public NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidbody;

        public bool isPerformingAction;
        public bool isInteracting;
      
       
        public float rotationSpeed = 15;
        public float maximumAttackRange = 1.5f;

        // public EnemyAttackAction[] enemyAttacks;
        // public EnemyAttackAction currentAttack;

        [Header("A.I Settings")]
        public float detectionRadius = 20;
        //The higher, lower, respectively these angles are, the greater detection FOV (like eye sight)
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
       

        public float currentRecoveryTime = 0;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyStats = GetComponent<EnemyStats>();
            enemyRigidbody = GetComponent<Rigidbody>();
            backStabCollider = GetComponentInChildren<BackStabCollider>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            navMeshAgent.enabled = false;
        }

        private void Start()
        {
            enemyRigidbody.isKinematic = false;
        }
        private void Update()
        {
            HandleRecoveryTimer();

            isInteracting = enemyAnimatorManager.anim.GetBool("isInteracting");
            enemyAnimatorManager.anim.SetBool("isDead", enemyStats.isDead);
        }

        //rigidbody movement better on fixed update
        private void FixedUpdate()
        {
            HandleStateMachine();

           
        }

        private void HandleStateMachine()
        {
            //OLD HandleCurrentAction
            ////call distancefromtarget before or its calculated as 0
            //if(enemyLocomotionManager.currentTarget != null)
            //{
            //    enemyLocomotionManager.distanceFromTarget = Vector3.Distance(enemyLocomotionManager.currentTarget.transform.position, transform.position);
            //}

            ////if no current target, try to find potential target
            //if(enemyLocomotionManager.currentTarget == null)
            //{
            //    enemyLocomotionManager.HandleDetection();
            //}
            //else if (enemyLocomotionManager.distanceFromTarget > enemyLocomotionManager.stoppingDistance)
            //{
            //    enemyLocomotionManager.HandleMoveToTarget();
            //}
            //else if (enemyLocomotionManager.distanceFromTarget <= enemyLocomotionManager.stoppingDistance)
            //{
            //    //Handle enemy attacks
            //    AttackTarget();
            //}

            if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

                if(nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }

        }

        private void SwitchToNextState(State state)
        {
            currentState = state;
        }

        private void HandleRecoveryTimer()
        {
            if(currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerformingAction)
            {
                if(currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }
        }

       



        private void OnDrawGizmosSelected()
        {
            //Gizmos.color = Color.red; 
            //Gizmos.DrawWireSphere(transform.position, detectionRadius);

            //lines
            Vector3 fovLine1 = Quaternion.AngleAxis(maximumDetectionAngle, transform.up) * transform.forward * detectionRadius;
            Vector3 fovLine2 = Quaternion.AngleAxis(minimumDetectionAngle, transform.up) * transform.forward * detectionRadius;
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, fovLine1);
            Gizmos.DrawRay(transform.position, fovLine2);
        }
    }
}
