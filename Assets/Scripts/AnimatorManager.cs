using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator animator;
        protected CharacterManager characterManager;
        protected CharacterStatsManager characterStatsManager;
        public bool canRotate;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
        }
        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("canRotate", canRotate);
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnim, 0.2f);
        }

        public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("isRotatingWithRootMotion", true);
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnim, 0.2f);
        }
        public virtual void TakeCriticalDamage()
        {
            characterStatsManager.TakeDamageNoAnimation(characterManager.pendingCriticalDamage);
            characterManager.pendingCriticalDamage = 0;
        }

        public virtual void CanRotate()
        {
            animator.SetBool("canRotate", true);
        }

        public virtual void StopRotate()
        {
            animator.SetBool("canRotate", false);

        }
        public virtual void EnableCombo()
        {
            animator.SetBool("CanDoCombo", true);
        }
        public virtual void DisableCombo()
        {
            animator.SetBool("CanDoCombo", false);
        }


        public virtual void EnableIsInvulnerable()
        {
            animator.SetBool("isInvulnerable", true);
        }
        public virtual void DisableIsInvulnerable()
        {
            animator.SetBool("isInvulnerable", false);

        }

        public virtual void EnableCanBeRiposted()
        {
            characterManager.canBeRiposte = true;
        }
        public virtual void DisableCanBeRiposted()
        {
            characterManager.canBeRiposte = false;

        }
        public virtual void EnableIsParring()
        {
            characterManager.isParrying = true;
        }
        public virtual void DisableIsParring()
        {
            characterManager.isParrying = false;
        }
    }
}
