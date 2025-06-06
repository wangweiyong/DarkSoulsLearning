using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        public string isUsingRightHand = "isUsingRightHand";
        public bool isUsingRightHandStatus = false;
        public string isUsingLeftHand = "isUsingLeftHand";
        public bool isUsingLeftHandStatus = false;

        public string isInvulnerable = "isInvulnerable";
        public bool isInvulnerableStatus = false;

        public string isInteractingBool = "isInteracting";
        public bool isInteractingStatus = false;

        public string isFiringSpellBool = "isFiringSpell";
        public bool isFiringSpellStatus = false;

        public string canRotateBool = "canRotate";
        public bool canRotateStatus = true;

        public string isRotatingWithRootMotion = "isRotatingWithRootMotion";
        public bool isRotatingWithRootMotionStatus = false;

        public string isMirroredBool = "isMirrored";
        public bool isMirroredStatus = false;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            CharacterManager character = animator.GetComponent<CharacterManager>();
            character.isUsingLeftHand = false;
            character.isUsingRightHand = false;
            animator.SetBool(isInvulnerable, isInvulnerableStatus);
            animator.SetBool(isInteractingBool, isInteractingStatus);
            animator.SetBool(isFiringSpellBool, isFiringSpellStatus);
            animator.SetBool(canRotateBool, canRotateStatus);
            animator.SetBool(isRotatingWithRootMotion, isRotatingWithRootMotionStatus);
            animator.SetBool(isMirroredBool, isMirroredStatus);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}