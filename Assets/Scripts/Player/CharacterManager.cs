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
        public BackStabCollider backStabCollider;
    }
}
