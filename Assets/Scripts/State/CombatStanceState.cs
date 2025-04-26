using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public PursueTargetState pursueTargetState;
        public EnemyAttackAction[] enemyAttacks;

        protected bool randomDestinationSet = false;
        protected float verticalMovementValue = 0;
        protected float horizontalMovementValue = 0;
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            enemyAnimatorManager.anim.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            enemyAnimatorManager.anim.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            attackState.hasPerformedAttack = false;
            if (enemyManager.isInteracting)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0);
                enemyAnimatorManager.anim.SetFloat("Horizontal", 0);
                return this;
            }
            else if (distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                return pursueTargetState;
            }
            //check for attack range,
            //potential circle player or walk aroung them
            //if in attack range return attack state
            //if we are in a cool down after attacking, return this state and continuous circleing player
            //if the player runs out of range return the pursuetarget state

            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                //decide circling action
                DecideCirclingAction(enemyAnimatorManager);
            }
            HandleRotateTowardsTarget(enemyManager);

            if(enemyManager.currentRecoveryTime <= 0 && attackState.currentAttack !=null)
            {
                randomDestinationSet = false;
                return attackState;
            }
            else
            {
                GetNewAttack(enemyManager);
               
            }
            return this;
        }
        private void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            //Rotate manually，攻击时的旋转，我们自己处理
            if (enemyManager.isPerfomingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
            //Rotate with pathfinding (navmesh)寻路中的旋转
            else
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.enemyRigidbody.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }
    
        private void DecideCirclingAction(EnemyAnimatorManager enemyAnimatorManager)
        {
            WalkAroungTarget(enemyAnimatorManager);
        }
        private void WalkAroungTarget(EnemyAnimatorManager enemyAnimatorManager)
        {
            verticalMovementValue = 0.5f;//Random.Range(0, 1); //only want to forward motion, so we always walk towards target
            
            if(verticalMovementValue <= 1 && verticalMovementValue > 0)
            {
                verticalMovementValue = 0.5f;
            }
            else if (verticalMovementValue >= -1 &&verticalMovementValue < 0)
            {
                verticalMovementValue = -0.5f;
            }

            horizontalMovementValue = Random.Range(-1, 1);
            if (horizontalMovementValue <= 1 && horizontalMovementValue > 0)
            {
                horizontalMovementValue = 0.5f;
            }
            else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
            {
                horizontalMovementValue = -0.5f;
            }
        }

        protected virtual void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            int maxScore = 0;
            for (int i = 0; i < enemyAttacks.Length; ++i)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && distanceFromTarget >= enemyAttackAction.minimumAttackAngle)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle &&
                        viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;
            for (int i = 0; i < enemyAttacks.Length; ++i)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && distanceFromTarget >= enemyAttackAction.minimumAttackAngle)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle &&
                        viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (attackState.currentAttack != null)
                        {
                            return;
                        }
                        temporaryScore += enemyAttackAction.attackScore;
                        if (temporaryScore > randomValue)
                        {
                            attackState.currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }
    }
}
