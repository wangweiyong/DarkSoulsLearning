using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

namespace wwy
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        EnemyManager enemyManager;

        public CharacterStats currentTarget;
        public LayerMask detectionLayers;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
        }
        public void HandleDetection()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayers);
        
            for(int i = 0; i < colliders.Length; ++i)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();
                if(characterStats != null )
                {
                    //CHECK FOR TEAM ID
                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if(viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        currentTarget = characterStats;
                    }
                }
            }
        }
    }
}
