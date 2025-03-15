using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class PlayerAnimatorManager : AnimatorManager
    {
        private PlayerManager playerManager;
        PlayerStats playerStats;
        private PlayerLocomotion playerLocomotion;
        int vertical;
        int horizontal;
        public bool canRotate;

        public void Initialze()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            playerStats = GetComponentInParent<PlayerStats>();
            anim = GetComponent<Animator>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float v = 0;
            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else v = 0;
            #endregion

            #region Horizontal
            float h = 0;
            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else h = 0;
            #endregion

            if (isSprinting)
            {
                v = 2f;
                h = horizontalMovement;
            }
            anim.SetFloat(vertical, v, 0.2f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.2f, Time.deltaTime);

        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotate()
        {
            canRotate = false;
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
            if(playerManager.isInteracting == false)
            {
                return;
            }

            float delta = Time.deltaTime;
            playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            Vector3 velocity = deltaPosition / delta;
            if(!playerManager.isJumping)
                velocity.y = Mathf.Clamp(playerLocomotion.rigidbody.velocity.y, -5f, 0f);
            playerLocomotion.rigidbody.velocity = velocity;
        }

        public override void TakeCriticalDamage()
        {
            playerStats.TakeDamageNoAnimation(playerManager.pendingCriticalDamage);
            playerManager.pendingCriticalDamage = 0;
        }
        public void SetIsInteracting()
        {
            anim.applyRootMotion = true;
            anim.SetBool("isInteracting", true);
        }
        public void ResetIsInteracting()
        {
            anim.applyRootMotion = false;
            anim.SetBool("isInteracting", false);
        }
    }
}
