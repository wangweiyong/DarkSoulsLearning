using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class PlayerAnimatorManager : AnimatorManager
    {
        private PlayerLocomotionManager playerLocomotionManager;
        int vertical;
        int horizontal;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponentInChildren<Animator>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
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
            animator.SetFloat(vertical, v, 0.2f, Time.deltaTime);
            animator.SetFloat(horizontal, h, 0.2f, Time.deltaTime);

        }

        private void OnAnimatorMove()
        {
            if(characterManager.isInteracting == false)
            {
                return;
            }

            float delta = Time.deltaTime;
            playerLocomotionManager.rigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            Vector3 velocity = deltaPosition / delta;
            if(!characterManager.isJumping)
                velocity.y = Mathf.Clamp(playerLocomotionManager.rigidbody.velocity.y, -5f, 0f);
            playerLocomotionManager.rigidbody.velocity = velocity;
        }

        public void SetIsInteracting()
        {
            animator.applyRootMotion = true;
            animator.SetBool("isInteracting", true);
        }
        public void ResetIsInteracting()
        {
            animator.applyRootMotion = false;
            animator.SetBool("isInteracting", false);
        }

        public void DisableCollision()
        {
            playerLocomotionManager.characterCollider.enabled = false;
            playerLocomotionManager.characterCollisionBlockerCollider.enabled = false;
        }
        public void EnableCollision()
        {
            playerLocomotionManager.characterCollider.enabled = true;
            playerLocomotionManager.characterCollisionBlockerCollider.enabled = true;
        }
    }
}
