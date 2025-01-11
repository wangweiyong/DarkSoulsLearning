using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotionManager enemyLocomotionManager;
        EnemyAnimatorManager enemyAnimatorManager;
        public bool isPerfomingAction;

        public EnemyAttackAction[] enemyAttacks;


        public EnemyAttackAction currentAttack;

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
        }

        private void Update()
        {
            HandleRecoveryTime();
        }
        private void FixedUpdate()
        {
            HandleCurrentAction();
        }

        private void HandleCurrentAction()
        {
            if(enemyLocomotionManager.currentTarget != null)
            {
                enemyLocomotionManager.distanceFromTarget = Vector3.Distance(enemyLocomotionManager.currentTarget.transform.position, transform.position);
            }
            if (enemyLocomotionManager.currentTarget == null)
            {
                enemyLocomotionManager.HandleDetection();
            }
            else if(enemyLocomotionManager.distanceFromTarget > enemyLocomotionManager.stoppingDistance)
            {
                enemyLocomotionManager.HandleMoveToTarget();
            }
            else if(enemyLocomotionManager.distanceFromTarget <= enemyLocomotionManager.stoppingDistance)
            {
                //Handle attack
                AttackTarget();

            }
        }

        #region Attacks
        private void GetNewAttack()
        {
            Vector3 targetDirection = enemyLocomotionManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            enemyLocomotionManager.distanceFromTarget = Vector3.Distance(enemyLocomotionManager.currentTarget.transform.position, transform.position);

            int maxScore = 0;
            for(int i = 0; i < enemyAttacks.Length; ++i)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if(enemyLocomotionManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && enemyLocomotionManager.distanceFromTarget >= enemyAttackAction.minimumAttackAngle)
                {
                    if(viewableAngle <= enemyAttackAction.maximumAttackAngle &&
                        viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;
            for(int i = 0; i < enemyAttacks.Length; ++i)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyLocomotionManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && enemyLocomotionManager.distanceFromTarget >= enemyAttackAction.minimumAttackAngle)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle &&
                        viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if(currentAttack != null)
                        {
                            return;
                        }
                        temporaryScore += enemyAttackAction.attackScore;
                        if(temporaryScore > randomValue)
                        {
                            currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }

        private void AttackTarget()
        {
            if (isPerfomingAction)
            {
                return;
            }
            
            if(currentAttack == null)
            {
                GetNewAttack();
            }
            else
            {
                isPerfomingAction = true;
                currentRecoveryTime = currentAttack.recoveryTime;
                enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                currentAttack = null;
            }
        }

        private void HandleRecoveryTime()
        {
            if(currentRecoveryTime >0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerfomingAction)
            {
                if(currentRecoveryTime <= 0)
                {
                    isPerfomingAction = false;
                }
            }
        }
        #endregion
    }
}
