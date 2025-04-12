using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        EnemyManager enemyManager;
        EnemyStats enemyStats;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
            enemyStats = GetComponentInParent<EnemyStats>();
        }
        public override void TakeCriticalDamage()
        {
            enemyStats.TakeDamageNoAnimation(enemyManager.pendingCriticalDamage);
            enemyManager.pendingCriticalDamage = 0;
        }
        public void AwardSoulsOnDeath()
        {
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();
            if (playerStats != null)
            {
                playerStats.AddSoulds(enemyStats.soulsAwardedOnDeath);
            }
            if(soulCountBar != null)
            {
                soulCountBar.SetSoulCountText(playerStats.soulCount);
            }
        }

        public void EnableIsParring()
        {
            enemyManager.isParrying = true;
        }
        public void DisableIsParring()
        {
            enemyManager.isParrying = false;
        }

        public void EnableCanBeRiposted()
        {
            enemyManager.canBeRiposte = true;
        }
        public void DisableCanBeRiposted()
        {
            enemyManager.canBeRiposte = false;

        }

        public void CanRotate()
        {
            anim.SetBool("canRotate", true);
        }

        public void StopRotate()
        {
            anim.SetBool("canRotate", false);

        }
        public void EnableCombo()
        {
            anim.SetBool("CanDoCombo", true);
        }
        public void DisableCombo()
        {
            anim.SetBool("CanDoCombo", false);
        }

        public void EnableIsInvulnerable()
        {
            anim.SetBool("isInvulnerable", true);
        }
        public void DisableIsInvulnerable()
        {
            anim.SetBool("isInvulnerable", false);

        }
        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidbody.velocity = velocity;
        }
    }
}
