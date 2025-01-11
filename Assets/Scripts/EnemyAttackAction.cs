using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    [CreateAssetMenu(menuName = "AI/Enemy Actions/Attack Action")]
    public class EnemyAttackAction : EnemyAction
    {
        // the higher the more likely to use this attack
        public int attackScore = 3;
        public float recoveryTime = 2;

        public float maximumAttackAngle = 35;
        public float minimumAttackAngle = -35;

        public float minimumDistanceNeededToAttack = 0;
        public float maximumDistanceNeededToAttack = 3;
    }
}
