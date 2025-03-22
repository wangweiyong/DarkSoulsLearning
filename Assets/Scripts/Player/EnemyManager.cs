using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Processors;

namespace wwy
{
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotionManager enemyLocomotionManager;
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyStats enemyStats;

        public CharacterStats currentTarget;
        public NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidbody;


        public bool isPerfomingAction;
        public bool isInteracting;

        public State currentState;

        public float rotationSpeed = 15;
        public float maximumAttackRange = 1.5f;
        [Header("Combat Flags")]
        public bool canDoCombo;

        [Header("AI Settings")]
        public float detectionRadius = 20;
        //the highter, and lower, respectively these angles are, the greater detection FIELD OF VIEW (
        //basically like eye sight)
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;

        public float currentRecoveryTime = 0;
        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyStats = GetComponent<EnemyStats>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigidbody = GetComponent<Rigidbody>();

            navMeshAgent.enabled = false;
            enemyRigidbody.isKinematic = false;

            backStabCollider = GetComponentInChildren<CriticalDamageCollider>();

        }

        private void Update()
        {
            HandleRecoveryTime();
            HandleStateMachine();
            isInteracting = enemyAnimatorManager.anim.GetBool("isInteracting");
            canDoCombo = enemyAnimatorManager.anim.GetBool("canDoCombo");
            enemyAnimatorManager.anim.SetBool("isDead", enemyStats.isDead);
        }
        private void FixedUpdate()
        {
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleStateMachine()
        {
            if(currentState != null)
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

        private void HandleRecoveryTime()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerfomingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPerfomingAction = false;
                }
            }
        }
    }
}
