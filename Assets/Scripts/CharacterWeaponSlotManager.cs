using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {

        [Header("Unarmed Weapon")]
        public WeaponItem unarmedWeapon;
        [Header("Weapon Slots")]
        public WeaponHolderSlots leftHandSlot;
        public WeaponHolderSlots rightHandSlot;
        public WeaponHolderSlots backSlot;

        [Header("Damage Collider")]
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;
    }
}
