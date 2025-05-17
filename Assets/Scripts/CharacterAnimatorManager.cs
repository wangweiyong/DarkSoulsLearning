using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace wwy
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        public Animator animator;
        protected CharacterManager characterManager;
        protected CharacterStatsManager characterStatsManager;
        public bool canRotate;

        protected RigBuilder rigBuilder;
        public TwoBoneIKConstraint leftHandConstraint;
        public TwoBoneIKConstraint rightHandConstraint;

        bool handIKWeightsRest = false;
        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            rigBuilder = GetComponent<RigBuilder>();
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
            characterStatsManager.TakeDamageNoAnimation(characterManager.pendingCriticalDamage, 0);
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
        
        public virtual void SetHandIKForWeapon(RightHandIKTarget rightHandTarget, LeftHandIKTarget leftHandTarget, bool isTwonHandingWeapon)
        {
            //check if we are two handing our weapon
            //if we are, apply handl ik if needed
            //assign hand ik to targets
            //if not disable hand ik for now
            if (isTwonHandingWeapon)
            {
                rightHandConstraint.data.target = rightHandTarget.transform;
                rightHandConstraint.data.targetPositionWeight = 1;
                rightHandConstraint.data.targetRotationWeight = 1;

                leftHandConstraint.data.target = leftHandTarget.transform;
                leftHandConstraint.data.targetPositionWeight = 1;
                leftHandConstraint.data.targetRotationWeight = 1;
            }
            else
            {
                rightHandConstraint.data.target = null;
                leftHandConstraint.data.target = null;
            }

            rigBuilder.Build();
        }
        public virtual void CheckHandIKWeight(RightHandIKTarget rightHandIK, LeftHandIKTarget leftHandIK, bool isTwoHandingWeaon)
        {
            if (characterManager.isInteracting) return;
            if (handIKWeightsRest)
            {
                handIKWeightsRest = false;
                if (rightHandConstraint.data.target != null)
                {
                    rightHandConstraint.data.target = rightHandIK.transform;
                    rightHandConstraint.data.targetPositionWeight = 1;
                    rightHandConstraint.data.targetRotationWeight = 1;
                }
                if (leftHandConstraint.data.target != null)
                {
                    leftHandConstraint.data.target = leftHandIK.transform;
                    leftHandConstraint.data.targetPositionWeight = 1;
                    leftHandConstraint.data.targetRotationWeight = 1;
                }
            }
        }

        public virtual void EraseHandIKForWeapon()
        {
            //reset all hand ik weights to 0
            handIKWeightsRest = true;
            if(rightHandConstraint.data.target != null)
            {
                rightHandConstraint.data.targetPositionWeight = 0;
                rightHandConstraint.data.targetRotationWeight = 0;
            }
            if(leftHandConstraint.data.target != null)
            {
                leftHandConstraint.data.targetPositionWeight = 0;
                leftHandConstraint.data.targetRotationWeight = 0;
            }
        }
    }
}
