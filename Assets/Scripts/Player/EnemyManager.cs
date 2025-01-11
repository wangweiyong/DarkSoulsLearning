using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotionManager enemyLocomotionManager;
        public bool isPerfomingAction;

        [Header("AI Settings")]
        public float detectionRadius = 20;
        //the highter, and lower, respectively these angles are, the greater detection FIELD OF VIEW (
        //basically like eye sight)
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        }

        private void FixedUpdate()
        {
            HandleCurrentAction();
        }

        private void HandleCurrentAction()
        {
            if(enemyLocomotionManager.currentTarget == null)
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

            }
        }

        #region Attacks


        #endregion
    }
}
