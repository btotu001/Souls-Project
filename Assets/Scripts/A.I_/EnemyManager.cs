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
        EnemyStatsManager enemyStatsmanager;
        EnemyEffectsManager enemyEffectsManager;

        public State currentState;
        public CharacterStatsManager currentTarget;

        public NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidbody;

        public bool isPerformingAction;
      
       
        public float rotationSpeed = 15;
        public float maximumAggroRadius = 1.5f;

        // public EnemyAttackAction[] enemyAttacks;
        // public EnemyAttackAction currentAttack;

        [Header("A.I Settings")]
        public float detectionRadius = 20;
        //The higher, lower, respectively these angles are, the greater detection FOV (like eye sight)
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
       
        public float currentRecoveryTime = 0;

        [Header("A.I Combat Settings")]
        public bool allowAIToPerformCombos;
        public float comboLikelyHood;
        public bool isPhaseShifting;
        public bool isBoss;
        public State idl;
        public State purs;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            enemyStatsmanager = GetComponent<EnemyStatsManager>();
            enemyEffectsManager = GetComponent<EnemyEffectsManager>();
            enemyRigidbody = GetComponent<Rigidbody>();
           
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
            HandleStateMachine();

            isRotatingWithRootMotion = enemyAnimatorManager.animator.GetBool("isRotatingWithRootMotion");
            isInteracting = enemyAnimatorManager.animator.GetBool("isInteracting");
            if (isBoss)
            {
                isPhaseShifting = enemyAnimatorManager.animator.GetBool("isPhaseShifting");
            }

            canDoCombo = enemyAnimatorManager.animator.GetBool("canDoCombo"); //when animatorManager enables, enable here too

            canRotate = enemyAnimatorManager.animator.GetBool("canRotate");
            enemyAnimatorManager.animator.SetBool("isDead", enemyStatsmanager.isDead);
        }

        private void FixedUpdate()
        {
            enemyEffectsManager.HandleAllBuildUpEffects();
        }

        //rigidbody movement better on fixed/late update
        private void LateUpdate()
        {
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;

        }
        
        public void ResetToPurs()
        {
            if (isBoss)
            {
                currentState = purs;
            }
        }
        public void ResetToPursStab()
        {
            PlayerStatsManager player;
            player = FindObjectOfType<PlayerStatsManager>();
            currentTarget = player;
            currentState = purs;
            
        }

        private void HandleStateMachine()
        {
           

            if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStatsmanager, enemyAnimatorManager);

                if(nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
            if(currentTarget != null && currentState == idl)
            {
                currentState = purs;
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
