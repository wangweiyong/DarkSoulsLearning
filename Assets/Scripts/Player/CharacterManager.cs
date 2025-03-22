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

        [Header("Combat Flags")]
        public bool canBeRiposte;
        public bool canBeParried;
        public bool isParrying;


        //Damage will be inflicted during an animation event
        //Used in backstab or riposte animations
        public int pendingCriticalDamage;
    }
}
