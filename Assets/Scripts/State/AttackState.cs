using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

namespace wwy
{
    public class AttackState : State
    {
        public CombatStanceState combatStanceState;
        public PursueTargetState pursueTargetState;
        public EnemyAttackAction[] enemyAttacks;

        public RotateTowardsTargetState rotateTowardsTargetState;

        bool willDoComboNextAttack = false;
        public bool hasPerformedAttack = false;

        public EnemyAttackAction currentAttack;
        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            RotateTowardsTargetWhilstAttacking(enemyManager);

            if(distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if(willDoComboNextAttack && enemyManager.canDoCombo)
            {
                //Attack with combo
                //set cool down time
                AttackTargetWithCombo(enemyAnimatorManager,enemyManager);
            }

            if (!hasPerformedAttack)
            {
                //Attack
                //rool for a combo check
                AttackTarget(enemyAnimatorManager, enemyManager);
                RollForComboChance(enemyManager);
            }

            if(willDoComboNextAttack && hasPerformedAttack)
            {
                return this;//goes back to perform the combo
            }

            return rotateTowardsTargetState;
        }

        private void AttackTarget(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
        {
            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyAnimatorManager.PlayWeaponTrailFX();
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformedAttack = true;
        }
        private void AttackTargetWithCombo(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
        {
            willDoComboNextAttack = false;
            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyAnimatorManager.PlayWeaponTrailFX();
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;
        }
        private void RotateTowardsTargetWhilstAttacking(EnemyManager enemyManager)
        {
            //Rotate manually，攻击时的旋转，我们自己处理
            if (enemyManager.canRotate)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }
        private void RollForComboChance(EnemyManager enemyManager)
        {
            float comboChance = Random.Range(0, 100);

            if(enemyManager.allowAIToPerformCombos && comboChance <= enemyManager.comboLikelyHodd)
            {
                if (currentAttack.comboAction!= null)
                {
                    willDoComboNextAttack = true;
                    currentAttack = currentAttack.comboAction;
                }
                else
                {
                    willDoComboNextAttack = false;
                    currentAttack = null;
                }
            }
        }
    }

}
