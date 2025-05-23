using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class RotateTowardsTargetState : State
    {
        public CombatStanceState combatStanceState;
        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            enemyAnimatorManager.animator.SetFloat("Vertical", 0);
            enemyAnimatorManager.animator.SetFloat("Horizontal", 0);

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

            if (enemyManager.isInteracting) return this;//when enter this state we will be interacting from the attack animation so we pause here until it has completed
            if(viewableAngle >= 100 && viewableAngle <=180 && !enemyManager.isInteracting)
            {
                enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
                return combatStanceState;
            }
            else if(viewableAngle <= -101 && viewableAngle >= 100 && !enemyManager.isInteracting)
            {
                enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
                return combatStanceState;
            }
            else if(viewableAngle <=-45 && viewableAngle >=-180 && !enemyManager.isInteracting)
            {
                enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Right", true);
                return combatStanceState;

            }
            else if(viewableAngle >=45 && viewableAngle <= 100 && !enemyManager.isInteracting)
            {
                enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Left", true);
                return combatStanceState;
            }

            return combatStanceState;
        }
    }
}
