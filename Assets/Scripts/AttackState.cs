using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class AttackState : State
    {
        public EnemyAttackAction[] enemyAttacks;


        public EnemyAttackAction currentAttack;
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            //select one of our many attacks based on attack scores
            //if the selected attack is not able to be used because of bad angles or distance, select a new attack
            //if the attack is visible, stop our movement and attack our target
            //set our recovery timer to the attacks revoery time;
            //return the combat stance state
            return this;
        }
    }
}
