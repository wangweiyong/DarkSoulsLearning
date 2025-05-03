using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("LockOn Transform")]
        public Transform lockOnTransform;

        [Header("Combat Colliders")]
        public BoxCollider backStabBoxCollider;
        public CriticalDamageCollider backStabCollider;
        public CriticalDamageCollider riposteCollider;

        [Header("IsInteracting")]
        public bool isInteracting;

        [Header("Combat Flags")]
        public bool canBeRiposte;
        public bool canBeParried;
        public bool canDoCombo;
        public bool isParrying;
        public bool isBlocking;
        public bool isInvulnerable;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;

        [Header("Movement Flgas")]
        public bool isRotateingWithRootMotion;
        public bool canRotate;
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool isJumping;


        [Header("Spells")]
        public bool isFiringSpell;

        //Damage will be inflicted during an animation event
        //Used in backstab or riposte animations
        public int pendingCriticalDamage;
    }
}
