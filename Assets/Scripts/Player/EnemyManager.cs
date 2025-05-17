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
        EnemyStatsManager enemyStatsManager;
        EnemyEffectsManager enemyEffectsManager;
        public CharacterStatsManager currentTarget;
        public NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidbody;


        public bool isPerfomingAction;

        public State currentState;

        public float rotationSpeed = 15;
        public float maximumAggroRadius = 1.5f;


        [Header("AI Settings")]
        public float detectionRadius = 20;
        //the highter, and lower, respectively these angles are, the greater detection FIELD OF VIEW (
        //basically like eye sight)
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
        public float currentRecoveryTime = 0;

        [Header("AI Combat Settings")]
        public float comboLikelyHodd = 50;
        public bool isPhaseShifting;
        public bool allowAIToPerformCombos;
        protected override void Awake()
        {
            base.Awake();
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            enemyStatsManager = GetComponent<EnemyStatsManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigidbody = GetComponent<Rigidbody>();
            enemyEffectsManager = GetComponent<EnemyEffectsManager>();
            navMeshAgent.enabled = false;
            enemyRigidbody.isKinematic = false;

            backStabCollider = GetComponentInChildren<CriticalDamageCollider>();

        }

        private void Update()
        {
            HandleRecoveryTime();
            HandleStateMachine();

            isUsingLeftHand = enemyAnimatorManager.animator.GetBool("isUsingLeftHand");
            isUsingRightHand = enemyAnimatorManager.animator.GetBool("isUsingRightHand");

            isInteracting = enemyAnimatorManager.animator.GetBool("isInteracting");
            isRotateingWithRootMotion = enemyAnimatorManager.animator.GetBool("isRotatingWithRootMotion");
            isPhaseShifting = enemyAnimatorManager.animator.GetBool("isPhaseShifting");
            isInvulnerable = enemyAnimatorManager.animator.GetBool("isInvulnerable");
            canDoCombo = enemyAnimatorManager.animator.GetBool("CanDoCombo");
            enemyAnimatorManager.animator.SetBool("isDead", enemyStatsManager.isDead);
            canRotate = enemyAnimatorManager.animator.GetBool("canRotate");
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            enemyEffectsManager.HandleAllBuildUpEffects();
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleStateMachine()
        {
            if(currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStatsManager, enemyAnimatorManager);
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
